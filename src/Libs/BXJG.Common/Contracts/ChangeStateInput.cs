using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;

namespace BXJG.Common.Contracts
{
    public class ChangeStateInput
    {
        [Required]
        public object Id { get; set; }
        [Required]
        public object State { get; set; }
    }

    public class ChangeStateInput<TId, TState>
    {
        public new TId Id
        {
            get
            {
                return (TId)(this as ChangeStateInput).Id;
            }
            set
            {
                (this as ChangeStateInput).Id = value;
            }
        }
        public TState State
        {
            get
            {
                return (TState)(this as ChangeStateInput).State;
            }
            set
            {
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