using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Specifications
{
   public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
    {
        /// <summary>
        /// Gets the first specification.
        /// </summary>
        public ISpecification<T> Left { get; }

        /// <summary>
        /// Gets the second specification.
        /// </summary>
        public ISpecification<T> Right { get; }

        /// <summary>
        /// Constructs a new instance of <see cref="CompositeSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">The first specification.</param>
        /// <param name="right">The second specification.</param>
        protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            Left = left;
            Right = right;
        }
    }
}
