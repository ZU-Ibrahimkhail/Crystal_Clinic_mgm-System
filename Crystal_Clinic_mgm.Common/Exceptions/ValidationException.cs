using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Crystal_Clinic_Mgm.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base()
        {
            Errors = new Dictionary<string, string[]>();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ValidationException(string message)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
           : base(message)
        {
        }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
        public IDictionary<string, string[]> Errors { get; }
        //
    }
}
