using System.Linq.Expressions;

namespace Application.Helper;

public static partial class FilterExpression
{
	public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
	{
		var parameter = Expression.Parameter(typeof(T), "x");
		Expression body = MakeComparison(propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property), comparison, value);
		return Expression.Lambda<Func<T, bool>>(body, parameter);
	}

	private static Expression MakeComparison(Expression left, string comparison, string value)
	{
		switch (comparison)
		{
			case "Equal":
				return MakeBinary(ExpressionType.Equal, left, value);

			case "NotEqual":
				return MakeBinary(ExpressionType.NotEqual, left, value);

			case "GreaterThan":
				return MakeBinary(ExpressionType.GreaterThan, left, value);

			case "GreaterThanOrEqual":
				return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);

			case "LessThan":
				return MakeBinary(ExpressionType.LessThan, left, value);

			case "LessThanOrEqual":
				return MakeBinary(ExpressionType.LessThanOrEqual, left, value);

			case "isnull":
				return MakeBinary(ExpressionType.Equal, left, value);

			case "isnotnull":
			case "startswith":
			case "endswith":
			case "contains":
				return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));

			case "doesnotcontain":
				return Expression.Not(Expression.Call(MakeString(left), "contains", Type.EmptyTypes, (Expression.Constant(value, typeof(string)))));

			default:
				return MakeBinary(ExpressionType.NotEqual, left, null);
		}
	}

	private static Expression MakeString(Expression source)
	{
		return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
	}

	private static Expression MakeBinary(ExpressionType type, Expression left, string value)
	{
		object typedValue = value;
		try
		{
			DateTime dt = Convert.ToDateTime(value);
			typedValue = dt;
		}
		catch { Console.WriteLine("not date"); typedValue = value; }

		if (left.Type != typeof(string))
		{
			if (string.IsNullOrEmpty(value))
			{
				typedValue = null;
				if (Nullable.GetUnderlyingType(left.Type) == null)
					left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
			}
			else
			{
				var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
				typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
					valueType == typeof(Guid) ? Guid.Parse(value) :
					Convert.ChangeType(value, valueType);
			}
		}
		var right = Expression.Constant(typedValue, left.Type);
		return Expression.MakeBinary(type, left, right);
	}
}