﻿namespace SharedKernel;

public readonly record struct Result
{
    public Error Error { get; } = Error.None;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    private Result(bool isSuccess)
    {
        Error = default;
        IsSuccess = isSuccess;
    }

    public static Result Success() => new(true);

    public static Result Failure(Error error) => new(error);
    
    
    public static Result Failure() => new(false);

    public static implicit operator Result(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

public readonly record struct Result<TValue>
{
    public TValue Value { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(string error)
    {
        Value = default;
        IsSuccess = false;
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(string error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(string error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TValue, TResult> failure)
        => IsSuccess ? success(Value!) : failure(Value!);
}

public readonly record struct Result<TValue, TError>
{
    public TValue Value { get; }
    public TError Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
    }

    private Result(TError error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }

    public static Result<TValue, TError> Success(TValue value) => new(value);

    public static Result<TValue, TError> Failure(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => Success(value);

    public static implicit operator Result<TValue, TError>(TError error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        => IsSuccess ? success(Value!) : failure(Error!);
}