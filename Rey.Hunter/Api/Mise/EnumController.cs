using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Attributes;
using Rey.Hunter.Models.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/[action]/{lang?}")]
    public class EnumController : ReyController {
        private List<dynamic> EnumToList<T>(string lang) where T : struct {
            var list = new List<dynamic>();
            var type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
                throw new InvalidOperationException("Not a enum type!");

            var values = Enum.GetValues(type);
            foreach (var value in values) {
                var name = Enum.GetName(type, value);
                var desc = type.GetTypeInfo().GetField(name).GetCustomAttributes<DescriptionAttribute>().FirstOrDefault(x => x.Language.Equals(lang ?? "en-us", StringComparison.CurrentCultureIgnoreCase));
                list.Add(new { name = name, value = value, desc = desc.Description, lang = desc.Language });
            }
            return list;
        }

        public Task<IActionResult> Gender(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<Gender>(lang);
            });
        }

        public Task<IActionResult> MaritalStatus(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<MaritalStatus>(lang);
            });
        }

        public Task<IActionResult> EducationLevel(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<EducationLevel>(lang);
            });
        }

        public Task<IActionResult> Language(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<Language>(lang);
            });
        }

        public Task<IActionResult> Nationality(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<Nationality>(lang);
            });
        }

        public Task<IActionResult> JobIntension(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<JobIntension>(lang);
            });
        }

        public Task<IActionResult> CompanyType(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<CompanyType>(lang);
            });
        }

        public Task<IActionResult> CompanyStatus(string lang) {
            return this.JsonInvokeManyAsync(() => {
                return EnumToList<CompanyStatus>(lang);
            });
        }
    }
}
