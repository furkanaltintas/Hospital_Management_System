using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HastaneProgrami.Security
{
    public class HastaneRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //throw new NotImplementedException();
            HastaneContext db = new HastaneContext();
            var hasta = db.hasta.FirstOrDefault(y => y.adi == username);
            if (hasta != null)
            {
                return new string[] { hasta.vatandasYetki.ad };
            }

            //var kullanici = db.kullanici.FirstOrDefault(x => x.adi == username);
            //return new string[] { kullanici.yetki.yetkiAd }; eski sistem

            var personel = db.personel.FirstOrDefault(x => x.adi == username);
            return new string[] { personel.yetki.yetkiAd };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}