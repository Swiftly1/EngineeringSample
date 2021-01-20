using System.Collections.Generic;

namespace Common
{
    public abstract class IMovable<T>
    {
        protected List<T> _Collection { get; set; } = new List<T>();

        protected int _Index { get; set; } = 0;

        protected T _Current => _Collection[_Index];

        protected bool MoveNext()
        {
            if (CanMoveNext())
            {
                _Index++;
                return true;
            }

            return false;
        }

        protected bool MoveBehind()
        {
            if (CanMoveBehind())
            {
                _Index--;
                return true;
            }

            return false;
        }

        protected bool CanMoveNext() => _Index + 1 < _Collection.Count;

        protected bool CanMoveBehind() => _Collection.Count > 0 && _Index > 0;

        protected bool HasPreviousElement() => _Index > 0;

        protected bool IsLast() => _Index == _Collection.Count - 1;

        protected T ElementAt(int index) => _Collection[index];
    }
}
