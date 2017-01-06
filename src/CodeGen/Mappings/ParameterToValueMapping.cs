using System;

namespace CodeGen.Mappings
{
    public class ParameterToValueMapping
    {
        public string Map(Parameter parameter)
        {
            if (parameter.DefaultValue == null)
            {
                return null;
            }

            //here you will actually have to map out what each of these things look like
            //from a formatting perspective
            //ie
            // "string"
            // new DateTime???
            // bool
            // Guid

            throw new NotImplementedException();

            //if (type == typeof(string))
            //{
            //    return item;
            //}
            //else if (type == typeof(int) || type == typeof(Nullable<int>))
            //{
            //    return Convert.ToInt32(item);
            //}
            //else if (type == typeof(long) || type == typeof(Nullable<long>))
            //{
            //    return Convert.ToInt64(item);
            //}
            //else if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
            //{
            //    DateTime result;

            //    if (DateTime.TryParse(item, out result))
            //    {
            //        return result;
            //    }
            //    else
            //    {
            //        throw new Exception("Couldn't parse datetime string");
            //    }
            //}
            //else if (type == typeof(DateTimeOffset) || type == typeof(Nullable<DateTimeOffset>))
            //{
            //    DateTimeOffset result;

            //    if (DateTimeOffset.TryParse(item, out result))
            //    {
            //        return result;
            //    }
            //    else
            //    {
            //        throw new Exception("Couldn't parse datetime string");
            //    }
            //}
            //else if (type == typeof(bool?) || type == typeof(bool))
            //{
            //    if (item.ToLower() == "true")
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else if (type == typeof(Guid) || type == typeof(Guid?))
            //{
            //    return Guid.Parse(item);
            //}
            //else
            //{
            //    throw new Exception("Unhandled object type: " + type.Name);
            //}
        }
    }
}
