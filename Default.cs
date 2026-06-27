using System;
using System.Collections.Generic;

namespace ChatbotWebForm
{
    public partial class Default : System.Web.UI.Page
    {
        SmartCybersecurityChatbot bot = new SmartCybersecurityChatbot();
        TaskRepository repo = new TaskRepository();
        QuizManager quiz = new QuizManager();

        private string PendingTask
        {
            get => Session["PendingTask"] as string ?? "";
            set => Session["PendingTask"] = value;
        }

        private bool WaitingForTaskDetails
        {
            get => Session["WaitingForTaskDetails"] as bool? ?? false;
            set => Session["WaitingForTaskDetails"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ChatHistory"] == null)
                    Session["ChatHistory"] = new List<string>();

                if (Session["UserName"] == null)
                    Session["UserName"] = "";
            }

            if (!IsPostBack && int.TryParse(Request.QueryString["delete"], out int id))
            {
                repo.DeleteTask(id);
                ActivityLog.Add("Task deleted");
                lblTasks.Text = "Task deleted successfully.";
            }
        }

     
        private string ExtractTask(string input)
        {
            input = input.ToLower();

            if (input.Contains("remind me to"))
                return input.Replace("remind me to", "").Trim();

            if (input.Contains("add task"))
                return input.Replace("add task", "").Trim();

            return input;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            var history = Session["ChatHistory"] as List<string>;
            if (history == null)
            {
                history = new List<string>();
                Session["ChatHistory"] = history;
            }

            string response;
            string intent = bot.DetectIntent(input);

            ActivityLog.Add("User: " + input);

            if (input.ToLower().StartsWith("my name is"))
            {
                string name = input.Substring(11).Trim();
                Session["UserName"] = name;
                response = "Nice to meet you " + name;
            }

            else if (intent == "quiz")
            {
                response = quiz.Start();
            }
            else if (input.Length == 1 && "abcd".Contains(input.ToLower()))
            {
                response = quiz.Answer(input);
            }

            else if (intent == "reminder" && !WaitingForTaskDetails)
            {
                PendingTask = ExtractTask(input);
                WaitingForTaskDetails = true;

                response =
                    "Task detected: " + PendingTask + "<br/><br/>" +
                    "Enter: description, YYYY-MM-DD<br/>" +
                    "Example: Update password, 2026-07-01";
            }

            else if (WaitingForTaskDetails)
            {
                string[] parts = input.Split(',');

                if (parts.Length >= 2)
                {
                    string description = parts[0].Trim();
                    string dateText = parts[1].Trim();

                   
                    string[] d = dateText.Split('-');

                    if (d.Length == 3 &&
                        int.TryParse(d[0], out int year) &&
                        int.TryParse(d[1], out int month) &&
                        int.TryParse(d[2], out int day))
                    {
                        try
                        {
                            DateTime date = new DateTime(year, month, day);

                            repo.AddTask(PendingTask, description, date);
                            ActivityLog.Add("Task created: " + PendingTask);

                            response =
                                "Task added successfully.<br/>" +
                                "Task: " + PendingTask + "<br/>" +
                                "Description: " + description + "<br/>" +
                                "Date: " + date.ToShortDateString();

                            PendingTask = "";
                            WaitingForTaskDetails = false;
                        }
                        catch
                        {
                            response = "Invalid calendar date.";
                        }
                    }
                    else
                    {
                        response = "Invalid format. Use YYYY-MM-DD.";
                    }
                }
                else
                {
                    response = "Please enter: description, date";
                }
            }

            else if (input.ToLower().Contains("activity log") ||
                     input.ToLower().Contains("what have you done"))
            {
                response = "Recent activity:<br/>" +
                           string.Join("<br/>", ActivityLog.GetLast(10));
            }

          
            else
            {
                response = bot.GetResponse(input);
            }

        
            string username = Session["UserName"] as string;

            if (!string.IsNullOrEmpty(username))
                response = "Hi " + username + ", " + response;

            
            history.Add("You: " + input);
            history.Add("Bot: " + response);

            lblChat.Text += "<b>You:</b> " + input + "<br/><b>Bot:</b> " + response + "<hr/>";
            txtInput.Text = "";
        }

        // clear current chat
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblChat.Text = "";
            Session["ChatHistory"] = new List<string>();
        }

        // add a task
        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            try
            {
                string dateText = txtDueDate.Text.Trim();
                string[] d = dateText.Split('-');

                if (d.Length == 3 &&
                    int.TryParse(d[0], out int year) &&
                    int.TryParse(d[1], out int month) &&
                    int.TryParse(d[2], out int day))
                {
                    DateTime date = new DateTime(year, month, day);

                    repo.AddTask(txtTaskName.Text, txtTaskDesc.Text, date);
                    ActivityLog.Add("Manual task added: " + txtTaskName.Text);

                    lblTasks.Text = "Task added successfully.";
                }
                else
                {
                    lblTasks.Text = "Invalid date format. Use YYYY-MM-DD.";
                }
            }
            catch
            {
                lblTasks.Text = "Error adding task.";
            }
        }

        // view tasks
        protected void btnViewTasks_Click(object sender, EventArgs e)
        {
            var tasks = repo.GetTasks();
            string output = "";

            foreach (var t in tasks)
            {
                output +=
                    "<b>Task:</b> " + t.TaskName + "<br/>" +
                    "<b>Description:</b> " + t.TaskDescription + "<br/>" +
                    "<b>Due Date:</b> " + t.DueDate.ToShortDateString() + "<br/>" +
                    "<a href='?delete=" + t.TaskID + "'>Delete</a><hr/>";
            }

            lblTasks.Text = tasks.Count == 0 ? "No tasks available." : output;
        }

        // clear away tasks
        protected void btnClearTasks_Click(object sender, EventArgs e)
        {
            foreach (var t in repo.GetTasks())
                repo.DeleteTask(t.TaskID);

            ActivityLog.Add("All tasks cleared");
            lblTasks.Text = "Tasks cleared successfully.";
        }
    }
}
