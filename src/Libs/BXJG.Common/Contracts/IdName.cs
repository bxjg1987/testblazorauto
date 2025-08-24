using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Contracts
{
    public class IdName
    {
        public object Id { get; set; }
        public string Name { get; set; }
        public IdName()
        {
        }
        public IdName(object id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class IdName<TKey> : IdName
    {
        private TKey _idCache;
        private bool _isIdCached = false;

        public IdName()
        {
        }

        public new TKey Id
        {
            get
            {
                if (!_isIdCached)
                {
                    _idCache = (TKey)base.Id;
                    _isIdCached = true;
                }
                return _idCache;
            }
            set
            {
                base.Id = value;
                _idCache = value;
                _isIdCached = true;
            }
        }

        public IdName(TKey id, string name) : base(id, name)
        {
            _idCache = id;
            _isIdCached = true;
        }
    }
}