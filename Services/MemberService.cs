using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseConsole.Services
{
    public class MemberService
    {
        public class MembershipInfo
        {
            public int MemberID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public int? MostSevereDiagnosisID { get; set; }

            public string MostSevereDiagnosticDescription { get; set; }

            public int? CategoryID { get; set; }

            public string CategoryDescription { get; set; }

            public int? CategoryScore { get; set; }

            public int IsMostSevereCategory { get; set; }

        }
        /// <summary>
        /// Looks up MemberDiagnosisInfoView based on Member ID
        /// Old School SQL Query because we only have a single table, and one query that is always the same
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static List<MembershipInfo> LookupMembershipInfo(int memberId)
        {
            var result = new List<MembershipInfo>();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("SELECT [MemberID] ,[FirstName] ,[LastName] ,[MostSevereDiagnosisID] ,[MostSevereDiagnosticDescription] ,[CategoryID] ,[CategoryDescription] ,[CategoryScore] ,[IsMostSevereCategory] FROM MemberDiagnosisInfoView WHERE MEMBERID = @MemberId", con);
                cmd.Parameters.AddWithValue("@MemberId",memberId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var r = new MembershipInfo();
                    if (!rdr.IsDBNull(0))
                        r.MemberID = rdr.GetInt32(0);
                    if (!rdr.IsDBNull(1))
                        r.FirstName = rdr.GetString(1);
                    if (!rdr.IsDBNull(2))
                        r.LastName = rdr.GetString(2);
                    if (!rdr.IsDBNull(3))
                        r.MostSevereDiagnosisID = rdr.GetInt32(3);
                    if(!rdr.IsDBNull(4))
                        r.MostSevereDiagnosticDescription = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                        r.CategoryID = rdr.GetInt32(5);
                    if(!rdr.IsDBNull(6))
                        r.CategoryDescription = rdr.GetString(6);
                    if (!rdr.IsDBNull(7))
                        r.CategoryScore = rdr.GetInt32(7);
                    if (!rdr.IsDBNull(8))
                        r.IsMostSevereCategory = rdr.GetInt32(8);
                    result.Add(r);
                   
                 
                }
                con.Close();
            }

            return result;
        }
        /// <summary>
        /// Provides response as list of strings to support console output
        /// </summary>
        /// <param name="membershipInfos"></param>
        /// <returns></returns>
        public static List<string> GetMembershipInfoResponse(List<MembershipInfo> membershipInfos)
        {
            var result = new List<string>();
            var header =
                "MemberID|FirstName|LastName|MostSevereDiagnosisID|MostSevereDiagnosticDescription|CategoryID|CategoryDescription|CategoryScore|IsMostSevereCategory";
            result.Add(header);
            result.Add("".PadLeft(header.Count(), '-'));
            var recordValues = membershipInfos.Select(mi => String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                mi.MemberID, mi.FirstName,
                mi.LastName, mi.MostSevereDiagnosisID, mi.MostSevereDiagnosticDescription, mi.CategoryID,
                mi.CategoryDescription, mi.CategoryScore, mi.IsMostSevereCategory)).ToList();
            recordValues.ForEach(ri =>
            {
                result.Add(ri);
                result.Add("".PadLeft(ri.Count(), '-'));
            });

            result.Add(String.Format("{0} Records Found", membershipInfos.Count));
            return result;

        }
    }
}
