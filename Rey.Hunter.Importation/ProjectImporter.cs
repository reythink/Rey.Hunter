using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using System;
using System.Linq;

namespace Rey.Hunter.Importation {
    public class ProjectImporter : ImporterBase<Project>, IAccountImporter<Project> {
        public ProjectImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var model = tool.CreateModel();
                    model.Account = account;
                    model.Id = tool.GetValue<string>(item, "_id");
                    model.CreateAt = tool.GetValue<DateTime?>(item, "CreateAt");

                    model.Position = (string)tool.GetValue(item, "Name");
                    model.Headcount = (int?)tool.GetValue(item, "Headcount");
                    model.Client = tool.FindOne<Company>(tool.GetValue<string>(item, "Client._id"));

                    model.Manager = tool.FindOne<User>((string)tool.GetValue(item, "Manager._id"));
                    model.Consultant.AddRange(tool.FindMany<User>(tool.GetIdList(item, "Consultants")).Select(x => (UserRef)x));

                    model.Function.AddRange(tool.FindMany<Function>(tool.GetIdList(item, "Functions")).Select(x => (FunctionRef)x));
                    model.Location.AddRange(tool.FindMany<Location>(tool.GetIdList(item, "Locations")).Select(x => (LocationRef)x));

                    model.AssignmentDate = (DateTime?)tool.GetValue(item, "AssignmentDate");
                    model.OfferSignedDate = (DateTime?)tool.GetValue(item, "OfferSignedDate");
                    model.OnBoardDate = (DateTime?)tool.GetValue(item, "OnBoardDate");

                    model.Notes = (string)tool.GetValue(item, "Notes");

                    foreach (var sub in item["Candidates"].AsBsonArray) {
                        var subModel = new ProjectCandidate();
                        subModel.Talent = tool.FindOne<Talent>(tool.GetValue<string>(sub, "Talent._id"));
                        subModel.Status = (CandidateStatus)(int)tool.GetValue(sub.AsBsonDocument, "Status");

                        foreach (var subSubItem in sub["Interviews"].AsBsonArray) {
                            subModel.Interviews.Add(new CandidateInterviewItem());
                        }

                        model.Candidate.Add(subModel);
                    }

                    model.Question.Question1 = (string)tool.GetValue(item, "JobUnderstanding.Field1");
                    model.Question.Question2 = (string)tool.GetValue(item, "JobUnderstanding.Field2");
                    model.Question.Question3 = (string)tool.GetValue(item, "JobUnderstanding.Field3");
                    model.Question.Question4 = (string)tool.GetValue(item, "JobUnderstanding.Field4");
                    model.Question.Question5 = (string)tool.GetValue(item, "JobUnderstanding.Field5");
                    model.Question.Question6 = (string)tool.GetValue(item, "JobUnderstanding.Field6");
                    model.Question.Question7 = (string)tool.GetValue(item, "JobUnderstanding.Field7");
                    model.Question.Question8 = (string)tool.GetValue(item, "JobUnderstanding.Field8");
                    model.Question.Question9 = (string)tool.GetValue(item, "JobUnderstanding.Field9");
                    model.Question.Question10 = (string)tool.GetValue(item, "JobUnderstanding.Field10");
                    model.Question.Question11 = (string)tool.GetValue(item, "JobUnderstanding.Field11");
                    model.Question.Question12 = (string)tool.GetValue(item, "JobUnderstanding.Field12");

                    tool.ImportAttachments(item, "Attachments", model.Attachment);
                }
            }
        }
    }
}