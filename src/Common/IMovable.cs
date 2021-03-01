﻿using System.Collections.Generic;

namespace Common
{
    public abstract class IMovable<T>
    {
        public IMovable(List<T> Collection)
        {
            _Collection = Collection;
        }

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

        protected List<T> TryGetAhead(int count)
        {
            if (count <= 0)
                return new List<T>();

            var list = new List<T>();

            while (count-- > 0 && MoveNext())
                list.Add(_Current);

            return list;
        }
    }
}
