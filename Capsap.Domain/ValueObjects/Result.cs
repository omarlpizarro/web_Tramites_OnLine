using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.ValueObjects
{
    /// <summary>
    /// Resultado de una operación sin valor de retorno
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; protected set; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Un resultado exitoso no puede tener un mensaje de error");

            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Un resultado fallido debe tener un mensaje de error");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Failure<T>(string error)
        {
            return new Result<T>(default(T), false, error);
        }

        // Combinar múltiples resultados
        public static Result Combine(params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailure)
                    return result;
            }
            return Success();
        }
    }

    /// <summary>
    /// Resultado de una operación con valor de retorno
    /// </summary>
    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        // IMPORTANTE: Estos métodos estáticos devuelven Result<T>, no Result
        public static new Result<T> Success(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static new Result<T> Failure(string error)
        {
            return new Result<T>(default(T), false, error);
        }

        // Conversión implícita de T a Result<T>
        public static implicit operator Result<T>(T value)
        {
            return Success(value);
        }

        // Método para transformar el valor si el resultado es exitoso
        public Result<TOutput> Map<TOutput>(Func<T, TOutput> mapper)
        {
            if (IsFailure)
                return Result<TOutput>.Failure(Error);

            try
            {
                var mappedValue = mapper(Value);
                return Result<TOutput>.Success(mappedValue);
            }
            catch (Exception ex)
            {
                return Result<TOutput>.Failure(ex.Message);
            }
        }

        // Método para encadenar operaciones que devuelven Result<T>
        public Result<TOutput> Bind<TOutput>(Func<T, Result<TOutput>> binder)
        {
            if (IsFailure)
                return Result<TOutput>.Failure(Error);

            try
            {
                return binder(Value);
            }
            catch (Exception ex)
            {
                return Result<TOutput>.Failure(ex.Message);
            }
        }
    }
}
