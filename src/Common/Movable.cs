using System.Linq;
using System.Collections.Generic;

namespace Common
{
    public abstract class Movable<T>
    {
        protected Movable(List<T> Collection)
        {
            _Collection = Collection;
        }

        protected List<T> _Collection { get; set; } = new();

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

        protected List<T> TakeToEnd(int offset = 0) => _Collection.Skip(_Index + offset).ToList();

        protected (bool Sucess, List<T> Items) TryGetAhead(int count, bool includeCurrent = false, bool movePointer = true)
        {
            var originalIndex = _Index;

            if (count <= 0)
                return (false, new List<T>());

            var list = new List<T>();

            if (!includeCurrent)
                MoveNext();

            do
            {
                list.Add(_Current);
            }
            while (list.Count < count && MoveNext());

            var success = list.Count == count;

            if (!success || !movePointer)
                _Index = originalIndex;

            return (success, list);
        }
    }
}
