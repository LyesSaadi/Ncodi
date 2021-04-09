﻿using System;

namespace Pital.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;

        public VariableSymbol Variable { get; }

        public override Type Type => Variable.Type;
    }
}
