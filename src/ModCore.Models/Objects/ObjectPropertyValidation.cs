using ModCore.Models.Enum;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.Objects
{
    public abstract class ObjectPropertyValidation
    {
        public abstract ValidationType Type { get; }

        public abstract bool IsValid(object value, PropertyValidationContext context);

        public abstract string GetValidationMessage();

        public ObjectPropertyValidation()
        {

        }

    }

    public class PropertyValidationContext
    {
        public ExpandoObject Object { get; set; }
    }

    public class ValidationResult
    {
        public bool Success { get; set; }

        public List<string> ErrorMessages { get; set; }

        public ValidationResult()
        {
            ErrorMessages = new List<string>();
            Success = true;
        }
    }

    public class PropertyHistory
    {
        private string _curHash;
        private string _prevHash;


        public string PropertyName { get; set; }

        public object NewValue { get; set; }

        public object OldValue { get; set; }

        public DateTime ChangedDate { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string HashHistoryChain
        {
            get { 
                if(_curHash == null)
                {
                    _curHash = ChainHash(_prevHash);
                }
                return _curHash;
            }
            set
            {
                _curHash = value;
            }
        }

        public PropertyHistory()
            :this(string.Empty)
        {
           
        }

        public PropertyHistory(string prevHash)
        {
            Attributes = new Dictionary<string, string>();
            _curHash = null;
            _prevHash = prevHash;
        }

        private string ChainHash(string prevHash)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var valueStrChain = string.Concat(prevHash, this.PropertyName, this.NewValue, this.OldValue, this.ChangedDate.ToString());

            var newHashBytes = Encoding.ASCII.GetBytes(valueStrChain);
            return Encoding.ASCII.GetString(sha1.ComputeHash(newHashBytes));
        }

    }

}
