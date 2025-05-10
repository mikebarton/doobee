using System;
using Doobee.Engine.Schema;
using Doobee.Parser.Expressions;

namespace Doobee.Engine.Engine.Processing.Extensions;

internal static class ColumnDefTypeExtensions
{
    public static int GetStorageSize(this ColumnDefType type) =>
        type switch
        {
            ColumnDefType.Int => sizeof(int),
            ColumnDefType.Text => sizeof(long),//text stores address in separate string file
            ColumnDefType.Bool => sizeof(bool),
            _ => throw new NotSupportedException()
        };
}