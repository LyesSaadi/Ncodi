﻿using System;

namespace Pital.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right)
        {
            Right = right;
            OperatorKind = operatorKind;
            Left = left;
        }


        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public override Type Type => Left.Type;

        public BoundExpression Right { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Left { get; }
    }
}
