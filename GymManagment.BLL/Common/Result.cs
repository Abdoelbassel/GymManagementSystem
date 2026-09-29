using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Common
{
    public sealed record Result(bool success, string? error = null, ResultKind kind = ResultKind.Ok)
    {
        public static Result Ok() => new(true);

        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);

        public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFound);

        public static Result ValidationError(string error) => new(false, error, ResultKind.ValidationError);
    }

    public sealed record Result<T>(bool success, T? value, string? error = null, ResultKind kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true, value);

        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, default, error, kind);

        public static Result<T> NotFound(string error = "NotFound") => new(false, default, error, ResultKind.NotFound);

        public static Result<T> ValidationError(string error) => new Result<T>(false, default, error, ResultKind.ValidationError);

        public static implicit operator T?(Result<T> result) => result.value;
    }
}
