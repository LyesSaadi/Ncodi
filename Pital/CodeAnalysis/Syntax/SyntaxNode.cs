﻿using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

namespace Pital.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }
        public virtual TextSpan Span
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBounds(first.Start,last.End);
            }
        }
        public IEnumerable<SyntaxNode> GetChildren() 
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in properties)
            {
                if (typeof(SyntaxNode).IsAssignableFrom(prop.PropertyType))
                {
                    var child = (SyntaxNode)prop.GetValue(this);
                    yield return child;
                }
                else if (typeof(IEnumerator<SyntaxNode>).IsAssignableFrom(prop.PropertyType))
                {
                    var children = (IEnumerable<SyntaxNode>)prop.GetValue(this);
                    foreach (var child in children)
                        yield return child;

                }
            }
        }

        public void WriteTo(TextWriter writer)
        {
            TreePrint(writer, this);
        }

        private static void TreePrint(TextWriter writer,SyntaxNode node, string indent = "", bool isLast = true)
        {
            // ├──
            // │    
            // └──

            var marker = isLast ? "└──" : "├──";
            writer.Write(indent);
            writer.Write(marker);
            writer.Write(node.Kind);
            if (node is SyntaxToken T && T.Value != null)
            {
                writer.Write(" ");
                writer.Write(T.Value);
            }

            writer.WriteLine();
            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                TreePrint(writer, child, indent, child == lastChild);
            }
        }

        public override string ToString()
        {
            using(var writer=new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }

    }
}
