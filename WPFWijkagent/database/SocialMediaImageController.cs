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
        /// sets image information in the databse
        /// </summary>
        /// <param name="image">data to put into the database</param>
        /// <returns>number of changed rows</returns>
        public int SetSocialMediaImage(SocialMediaImage image)
        {
            SqlCommand query = new SqlCommand("" +
                $"INSERT INTO {Table} ({ColMessageID}, {ColURL}) " +
                $"VALUES (@{ColMessageID}, @{ColURL})");

            // prepare values in statement
            query.Parameters.Add($"@{ColMessageID}", System.Data.SqlDbType.Int);
            query.Parameters.Add($"@{ColURL}", System.Data.SqlDbType.VarChar);
            query.Parameters[$"@{ColMessageID}"].Value = image.SocialMediaMessageID;
            query.Parameters[$"@{ColURL}"].Value = image.URL;

            return _dbContext.ExecuteQuery(query);
        }

        /// <summary>
        /// creates a SocialMediaImage from information from the database
        /// </summary>
        /// <param name="socialMediaImageData">data to be used</param>
        /// <returns>created image</returns>
        private SocialMediaImage ObjectArrayToSocialMediaImage(object[] socialMediaImageData) => new SocialMediaImage
        {
            SocialMediaMessageID = (int)socialMediaImageData[0],
            URL = socialMediaImageData[1].ToString()
        };

        /// <summary>
        /// searches the database for images from a certain message
        /// </summary>
        /// <param name="messageID">id of the message of wich to obtain the images</param>
        /// <returns>a list of images</returns>
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
