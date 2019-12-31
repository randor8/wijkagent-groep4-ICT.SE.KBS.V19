using System.Collections.Generic;
using System.Data.SqlClient;
using WijkagentModels;

namespace WijkagentWPF.database
{
    public class SocialMediaImageController
    {
        private readonly DBContext _dbContext = new DBContext();

        private const string Table = "SocialMediaImage";
        private const string ColMessageID = "SocialMediaMessageID";
        private const string ColURL = "URL";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="url"></param>
        /// <returns>inserted image</returns>
        public SocialMediaImage SetSocialMediaImage(int messageID, string url)
        {
            SqlCommand query = new SqlCommand("" +
                $"INSERT INTO {Table} ({ColMessageID}, {ColURL}) " +
                $"VALUES (@{ColMessageID}, @{ColURL})");

            // prepare values in statement
            query.Parameters.Add($"@{ColMessageID}", System.Data.SqlDbType.Int);
            query.Parameters.Add($"@{ColURL}", System.Data.SqlDbType.VarChar);
            query.Parameters[$"@{ColMessageID}"].Value = messageID;
            query.Parameters[$"@{ColURL}"].Value = url;

            _dbContext.ExecuteQuery(query);

            return new SocialMediaImage
            {
                SocialMediaMessageID = messageID,
                URL = url
            };
        }

        private SocialMediaImage ObjectArrayToSocialMediaImage(object[] socialMediaImageData) => new SocialMediaImage
        {
            SocialMediaMessageID = (int)socialMediaImageData[0],
            URL = socialMediaImageData[1].ToString()
        };

        public List<SocialMediaImage> GetSocialMediaImages(int messageID)
        {
            SqlCommand query = new SqlCommand($"SELECT {ColMessageID}, {ColURL} " +
                $"FROM {Table} WHERE {ColMessageID} = @{ColMessageID}");

            // prepare values in statement
            query.Parameters.Add($"@{ColMessageID}", System.Data.SqlDbType.Int);
            query.Parameters[$"@{ColMessageID}"].Value = messageID;

            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);
            List<SocialMediaImage> socialMediaImages = new List<SocialMediaImage>();

            rows.ForEach(smi => socialMediaImages.Add(ObjectArrayToSocialMediaImage(smi)));
            return socialMediaImages;
        }
    }
}
