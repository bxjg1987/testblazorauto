using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;

namespace BXJG.Common.Contracts
{
    public  class ChangeStateInput
    {
        [Required]
        public virtual object Id { get; set; }
        [Required]
        public virtual object State { get; set; }
    }

    public class ChangeStateInput<TId, TState> : ChangeStateInput
    {
        TId _id;
        public new TId Id
        {
            get
            {
                //拆箱
                //return (TId)(this as ChangeStateInput).Id;
                return _id;
            }
            set
            {
                _id = value;
                //装箱
                (this as ChangeStateInput).Id = value;
            }
        }

        TState _state;
        public new TState State
        {
            get
            {
                return _state;
                //return (TState)(this as ChangeStateInput).State;
            }
            set
            {
                _state = value;
                (this as ChangeStateInput).State = value;
            }
        }
    }



    public class ChangeStringStateInput<TState> : ChangeStateInput<string, TState> { }
    public class ChangeIntStateInput<TState> : ChangeStateInput<int, TState> { }
    public class ChangeLongStateInput<TState> : ChangeStateInput<long, TState> { }
    public class ChangeGuidStateInput<TState> : ChangeStateInput<Guid, TState> { }




    public class ChangeStringBoolInput : ChangeStringStateInput< bool> { }
    public class ChangeIntBoolInput : ChangeIntStateInput< bool> { }
    public class ChangeLongBoolInput : ChangeLongStateInput<bool> { }
    public class ChangeGuidBoolInput : ChangeGuidStateInput< bool> { }

}