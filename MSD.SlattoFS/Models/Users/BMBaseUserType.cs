using System.Collections.Generic;
using Umbraco.Core.Models.EntityBase;
using Umbraco.Core.Models.Membership;

namespace MSD.SlattoFS.Models.Users
{
    public class BMBaseUserType : Entity, IUserType
    {
        private string _alias;
        public string Alias
        {
            get
            {
                return _alias;
            }

            set
            {
                _alias = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        private IEnumerable<string> _permissions;
        public IEnumerable<string> Permissions
        {
            get
            {
                return _permissions;
            }

            set
            {
                _permissions = value;
            }
        }
    }
}