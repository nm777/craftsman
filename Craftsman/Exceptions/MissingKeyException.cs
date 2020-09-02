namespace Craftsman.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    class MissingKeyException : Exception
    {
        public MissingKeyException() : base($"One of your entity properties is missing a primary or composite key designation. " +
            $"Please make sure you have an `IsPrimaryKey: true` option on whichever property you want to be used as your primary key or, if using composite keys, set `IsCompositeKey` to `true` for at least 2 of the properties.")
        {

        }
    }
}
