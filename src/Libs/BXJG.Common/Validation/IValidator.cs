using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Validation
{
    public class ValidationResult
    {
        public ValidationResult(bool success, string errorMessage = default)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
    }
    public interface IValidator
    {
        ValueTask<ValidationResult> ValidateAsync(object obj);
    }
    public abstract class Validator<T> : IValidator
    {
        public ValueTask<ValidationResult> ValidateAsync(object obj)
        {
            var o = (T)Convert.ChangeType(obj, typeof(T));
            return ValidateAsync(o);
        }

        protected abstract ValueTask<ValidationResult> ValidateAsync(T obj);
    }
    public class StringLengthValidator : Validator<string>
    {
        public StringLengthValidator(uint maxLength)
        {
            MaxLength = maxLength;
        }

        public StringLengthValidator(uint minLength, uint maxLength) : this(maxLength)
        {
            MinLength = minLength;
        }

        public uint MinLength { get; set; }
        public uint MaxLength { get; set; }

        protected override ValueTask<ValidationResult> ValidateAsync(string obj)
        {
            var b = obj.Length >= MinLength && obj.Length < MaxLength;
            string msg = string.Empty;
            if (b)
                return ValueTask.FromResult(new ValidationResult(b));
            return ValueTask.FromResult(new ValidationResult(b, ""));
        }
    }
}
