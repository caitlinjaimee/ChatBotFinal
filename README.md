# ChatBotFinal
ChatBot  
The chatbot is designed to simulate a smart assistant that responds to user queries, remembers conversation context, and manages simple tasks using a SQL Server database. ChatBot System Keyword-based cybersecurity responses topic detection (passwords, phishing, VPN, malware,help) Conversation history 


ChatBot  
Keyword-based cybersecurity responses 
Topic detection (passwords, phishing, VPN, malware, help) 
Conversation history 


Memory & Recall 
Stores chat history during session 
Recalls previous conversation (history) 
Remembers last topic discussed 
Basic conversational flow simulation 


Natural Language Processing  
Recognises keys words like 
“remind ” 
“add ” 
“quiz” 
“activity log” 

Supports Actions like 
Detect task request 
Ask for missing task details 
Save completed task to database 


Quiz Game 
Cybersecurity mini-quiz with 10 questions 
Multiple-choice  (A–D) 
Tracks score during session 
Final score summary at the end of quiz 


Task Manager 
Add tasks with description and due date
Add tasks directly through chatbot or task manager panel 
View all tasks in database 
Delete individual tasks using query string 
Clear all tasks 
SQL Server integration (ADO.NET) 


Activity Log 
Records Actions like 
Tasks created 
Tasks deleted 
Chat interactions 
Quiz usage 
NLP detections 



Database Setup 
Create Database 

CREATE DATABASE ChatbotDB; 
GO 
USE ChatbotDB; 
Create Tasks Table 

CREATE TABLE Tasks 
( 
   TaskID INT IDENTITY(1,1) PRIMARY KEY, 
   TaskName NVARCHAR(100) NOT NULL, 
   TaskDescription NVARCHAR(500), 
   DueDate DATE NOT NULL 
); 
 
