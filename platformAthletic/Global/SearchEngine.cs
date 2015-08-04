using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using platformAthletic.Model;
using platformAthletic.Tools;

namespace platformAthletic.Global
{
    public class SearchEngine
    {
        public static IEnumerable<User> Search(string SearchString, IQueryable<User> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = 0;
                
                if (entry.Email != null)
                {
                    rank += Regex.Matches(entry.Email.ToLowerInvariant(), regex).Count;
                }
                if (entry.FirstName != null)
                {
                    rank += Regex.Matches(entry.FirstName.ToLowerInvariant(), regex).Count;
                }
                if (entry.LastName != null)
                {
                    rank += Regex.Matches(entry.LastName.ToLowerInvariant(), regex).Count;
                }
                if (entry.PhoneNumber != null)
                {
                    rank += Regex.Matches(entry.PhoneNumber.ToLowerInvariant(), regex).Count;
                }
                if (entry.TeamOfPlay != null)
                {
                    rank += Regex.Matches(entry.TeamOfPlay.Name.ToLowerInvariant(), regex).Count;
                }
                if (entry.OwnTeam != null)
                {
                    rank += Regex.Matches(entry.OwnTeam.Name.ToLowerInvariant(), regex).Count;
                }
                rank += Regex.Matches(entry.Role.ToLowerInvariant(), regex).Count;
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<SBCValue> Search(string SearchString, IQueryable<SBCValue> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = 0;

                if (entry.User.FirstName != null)
                {
                    rank += Regex.Matches(entry.User.FirstName.ToLowerInvariant(), regex).Count;
                }
                if (entry.User.LastName != null)
                {
                    rank += Regex.Matches(entry.User.LastName.ToLowerInvariant(), regex).Count;
                }
                rank += Regex.Matches(entry.AddedDate.ToString("d"), regex).Count;
                rank += Regex.Matches(entry.Squat.ToString(), regex).Count;
                rank += Regex.Matches(entry.Bench.ToString(), regex).Count;
                rank += Regex.Matches(entry.Clean.ToString(), regex).Count;
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<User> SearchTeamUser(string SearchString, IQueryable<User> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = 0;

                if (entry.FirstName != null)
                {
                    rank += Regex.Matches(entry.FirstName.ToLowerInvariant(), regex).Count;
                }
                if (entry.LastName != null)
                {
                    rank += Regex.Matches(entry.LastName.ToLowerInvariant(), regex).Count;
                }
                if (entry.FieldPositions.Any())
                {
                    foreach (var fieldPosition in entry.FieldPositions)
                    {
                        if (fieldPosition != null)
                        {
                            if (fieldPosition.Name != null)
                            {
                                rank += Regex.Matches(fieldPosition.Name.ToLowerInvariant(), regex).Count;
                            }
                            if (fieldPosition.Code != null)
                            {
                                rank += Regex.Matches(fieldPosition.Code.ToLowerInvariant(), regex).Count;
                            }
                        }
                    }
                }

                rank += Regex.Matches(entry.Squat.ToString(), regex).Count;
                rank += Regex.Matches(entry.Bench.ToString(), regex).Count;
                rank += Regex.Matches(entry.Clean.ToString(), regex).Count;
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }
    }
}