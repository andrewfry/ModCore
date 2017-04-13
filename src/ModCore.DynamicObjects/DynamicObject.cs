using ModCore.Models.Objects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ModCore.DynamicObjects
{
    public class DynamicObject
    {
        private DynamicObjectTemplate _template;
        private ExpandoObject _object;
        private IDictionary<string, object> _objectDic { get { return _object as IDictionary<string, object>; } }
        private Dictionary<ObjectProperty, Stack<PropertyHistory>> _propertyHistory;

        public Dictionary<ObjectProperty, Stack<PropertyHistory>> PropertyHistory { get { return _propertyHistory; } }

        public Dictionary<ObjectProperty, object> Properties
        {
            get
            {
                var dict = new Dictionary<ObjectProperty, object>();
                foreach (var p in _template.Properties)
                {
                    var value = _objectDic[p.Name] == null ? p.DefaultValue : _objectDic[p.Name];
                    dict.Add(p, value);
                }
                return dict;
            }
        }

        public ValidationResult UpdateProperty(string propertyName, object value)
        {
            var prop = Properties.Single(a => a.Key.Name == propertyName).Key;
            return UpdateProperty(prop, value);
        }

        public ValidationResult UpdateProperty(ObjectProperty property, object value, Dictionary<string, string> Attributes = null)
        {
            var result = new ValidationResult();
            var context = new PropertyValidationContext();
            context.Object = _object;

            foreach (var v in property.Validation)
            {
                if (!v.IsValid(value, context))
                {
                    result.Success = false;
                    result.ErrorMessages.Add(v.GetValidationMessage());
                }
            }
            var oldValue = _objectDic[property.Name];
            _objectDic[property.Name] = value;

            OnPropertyChanged(property, value, oldValue);

            return result;
        }

        private void RecordPropertyHistory(ObjectProperty property, object newValue, object oldValue, Dictionary<string, string> Attributes)
        {
            var prevProperty = this.PropertyHistory.SingleOrDefault(a => a.Key.Name == property.Name);

            PropertyHistory propHist = null;

            if (prevProperty.Value != null)
                propHist = new PropertyHistory(prevProperty.Value.First().HashHistoryChain);
            else
            {
                propHist = new PropertyHistory();
            }


            propHist.ChangedDate = DateTime.Now;
            propHist.NewValue = newValue;
            propHist.OldValue = oldValue;
            propHist.PropertyName = property.Name;
            propHist.Attributes = Attributes;

            if (prevProperty.Value != null)
            {

            }
            else
            {
                //_propertyHistory.Add(property, new Stack<PropertyHistory>(propHist));
            }
        }

        /// <summary>
        /// Sends in the property, the new value, and the old value
        /// </summary>
        public Action<ObjectProperty, object, object> OnPropertyChanged;

        public DynamicObject(DynamicObjectTemplate template, ExpandoObject obj)
        {
            _template = template;
            _object = obj;
            _propertyHistory = new Dictionary<ObjectProperty, Stack<PropertyHistory>>();
        }


    }
}
