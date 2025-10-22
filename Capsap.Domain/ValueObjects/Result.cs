using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.ValueObjects
{
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
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return Success(value);
        }
    }

}
