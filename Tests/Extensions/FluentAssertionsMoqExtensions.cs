using Moq;
using System.Linq.Expressions;

namespace Tests.Extensions;

public static class FluentAssertionsMoqExtensions
{
    public static void ShouldNotHaveBeenCalled<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
    {
        mock.Verify(expression, Times.Never, $"Expected method {expression} to not have been called, but it was.");
    }

    public static void ShouldNotHaveBeenCalled<T>(this Mock<T> mock, Expression<Action<T>> expression) where T : class
    {
        mock.Verify(expression, Times.Never, $"Expected method {expression} to not have been called, but it was.");
    }

    public static void ShouldHaveBeenCalled<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> expression, Func<Times> times) where T : class
    {   
        mock.Verify(expression, times, $"Expected method {expression} to have been called {times}, but it wasn't.");
    }

    public static void ShouldHaveBeenCalled<T>(this Mock<T> mock, Expression<Action<T>> expression, Func<Times> times) where T : class
    {   
        mock.Verify(expression, times, $"Expected method {expression} to have been called {times}, but it wasn't.");
    }
}