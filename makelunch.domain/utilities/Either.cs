using System;

namespace makeLunch.domain.utilities
{
    /// <summary>
    /// Either monad implementation
    /// Value can only be left or right value
    /// </summary>
    /// <typeparam name="TL">The type of the l.</typeparam>
    /// <typeparam name="TR">The type of the r.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "L")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "R")]
    public class Either<TL, TR>
    {
        private readonly TL _left;
        private readonly TR _right;
        private readonly bool _isLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TL, TR}" /> class.
        /// </summary>
        /// <param name="left">The left.</param>
        public Either(TL left)
        {
            _left = left;
            _isLeft = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TL, TR}"/> class.
        /// </summary>
        /// <param name="right">The right.</param>
        public Either(TR right)
        {
            _right = right;
            _isLeft = false;
        }

        /// <summary>
        /// Matches the specified left or right function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftFunc">The left function.</param>
        /// <param name="rightFunc">The right function.</param>
        /// <returns></returns>
        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        {
            if (leftFunc == null) throw new ArgumentNullException(nameof(leftFunc));
            if (rightFunc == null) throw new ArgumentNullException(nameof(rightFunc));
            return _isLeft ? leftFunc(_left) : rightFunc(_right);
        }

        /// <summary>
        /// Matches the specified left action.
        /// </summary>
        /// <param name="leftAction">The left action.</param>
        /// <param name="rightAction">The right action.</param>
        /// <exception cref="System.ArgumentNullException">
        /// leftAction
        /// or
        /// rightAction
        /// </exception>
        public void Match(Action<TL> leftAction, Action<TR> rightAction)
        {
            if (leftAction == null) throw new ArgumentNullException(nameof(leftAction));
            if (rightAction == null) throw new ArgumentNullException(nameof(rightAction));
            if (_isLeft) { leftAction(_left); } else { rightAction(_right); }
        }
    }

    /// <summary>
    /// Factory class that removes verbosity of creating either instance
    /// </summary>
    /// <typeparam name="TL">The type of the l.</typeparam>
    /// <typeparam name="TR">The type of the r.</typeparam>
    public class EitherFactory<TL, TR>
    {
        /// <summary>
        /// Creates an instance with the left value.
        /// </summary>
        /// <typeparam>The type of the l.</typeparam>
        /// <typeparam>The type of the r.</typeparam>
        public Either<TL,TR> Create(TL left)
        {
            return new Either<TL, TR>(left);
        }

        /// <summary>
        /// Creates an instance with the right value.
        /// </summary>
        /// <typeparam>The type of the l.</typeparam>
        /// <typeparam>The type of the r.</typeparam>
        public Either<TL,TR> Create(TR right)
        {
            return new Either<TL,TR>(right);
        }
    }
}
