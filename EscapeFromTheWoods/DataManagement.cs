using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    public class DataManagement
    {
        private string connectionString;

        public DataManagement(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public async Task AddForest(int woodId, List<Tree> treeList)
        {
            Console.WriteLine($"write wood {woodId} to database - start");
            SqlConnection connection = GetConnection();
           
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("woodID", typeof(int));
                        table.Columns.Add("treeID", typeof(int));
                        table.Columns.Add("x", typeof(int));
                        table.Columns.Add("y", typeof(int));

                        foreach (Tree tree in treeList)
                        {
                            table.Rows.Add(woodId, tree.treeID, tree.x, tree.y);
                        }

                        bulkCopy.DestinationTableName = "dbo.WoodRecords";
                        bulkCopy.ColumnMappings.Add("woodID", "woodID");
                        bulkCopy.ColumnMappings.Add("treeID", "treeID");
                        bulkCopy.ColumnMappings.Add("x", "x");
                        bulkCopy.ColumnMappings.Add("y", "y");
                        bulkCopy.WriteToServer(table);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine($"write wood {woodId} to database - end");
                }
            }
        }

        public async Task AddMonkeyRecord(int woodId, Monkey monkey, List<Tree> treeRoute)
        {
            Console.WriteLine($"write route to database wood : {woodId} , monkey : {monkey.ID} - start");
            SqlConnection connection = GetConnection();

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("monkeyId", typeof(int));
                        table.Columns.Add("monkeyName", typeof(string));
                        table.Columns.Add("woodID", typeof(int));
                        table.Columns.Add("seqnr", typeof(int));
                        table.Columns.Add("treeID", typeof(int));
                        table.Columns.Add("x", typeof(int));
                        table.Columns.Add("y", typeof(int));

                        for (int i = 0; i < treeRoute.Count; i++)
                        {
                            table.Rows.Add(monkey.ID, monkey.name, woodId, i+1, treeRoute[i].treeID, treeRoute[i].x, treeRoute[i].y);
                        }

                        bulkCopy.DestinationTableName = "dbo.MonkeyRecords";
                        bulkCopy.ColumnMappings.Add("monkeyId", "monkeyId");
                        bulkCopy.ColumnMappings.Add("monkeyName", "monkeyName");
                        bulkCopy.ColumnMappings.Add("woodID", "woodID");
                        bulkCopy.ColumnMappings.Add("seqnr", "seqnr");
                        bulkCopy.ColumnMappings.Add("treeID", "treeID");
                        bulkCopy.ColumnMappings.Add("x", "x");
                        bulkCopy.ColumnMappings.Add("y", "y");
                        bulkCopy.WriteToServer(table);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine($"write route to database wood : {woodId} , monkey : {monkey.ID} - stop");
                }
            }
        }

        public async Task AddLog(int woodId, Dictionary<Monkey, List<Tree>> monkeysTreeRoute)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO dbo.Logs (woodID, monkeyID, message) output INSERTED.ID VALUES(@woodID, @monkeyID, @message)";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@woodID", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@monkeyID", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@woodID"].Value = woodId;
                    foreach (Monkey monkey in monkeysTreeRoute.Keys)
                    {
                        command.Parameters["@monkeyID"].Value = monkey.ID;
                        foreach (Tree tree in monkeysTreeRoute[monkey])
                        {
                            command.Parameters["@message"].Value = $"{monkey.name} is now in tree {tree.treeID} at location ({tree.x}, {tree.y})";
                            command.ExecuteNonQuery();
                        }
                    }                    
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
