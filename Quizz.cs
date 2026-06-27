using System;
using System.Collections.Generic;

namespace ChatbotWebForm
{
    public class QuizManager
    {
        private int score = 0;
        private int index = 0;

        private List<(string Q, string A, string A1, string A2, string A3, string A4, string Explain)> questions =
            new List<(string, string, string, string, string, string, string)>
        {

//quizz game, this is readable for user with clear options

            ("What is phishing?",
                "b",
                "A fake website",
                "A scam email or message",
                "A firewall tool",
                "A password manager",
                "Phishing is fake messages trying to steal your data."),

            ("A strong password should include?",
                "d",
                "Only letters",
                "Only numbers",
                "Your name",
                "Letters, numbers and symbols",
                "Strong passwords include a mix of characters."),

            ("What is 2FA?",
                "a",
                "Two-step verification",
                "A virus scanner",
                "A browser extension",
                "A firewall rule",
                "2FA adds an extra layer of security."),

            ("VPN is used for?",
                "b",
                "Speeding up games",
                "Encrypting internet traffic",
                "Deleting viruses",
                "Blocking ads",
                "VPN protects your data by encryption."),

            ("Clicking unknown links is?",
                "b",
                "Safe",
                "Dangerous",
                "Required",
                "Recommended",
                "Unknown links may contain malware.")
        };

        public string Start()
        {
            score = 0;
            index = 0;
            return "Cybersecurity Quiz Started! Good luck 👍\n\n" + GetQuestion();
        }

        public string GetQuestion()
        {
            if (index >= questions.Count)
            {
                return "Quiz Completed!\nFinal Score: " + score + "/" + questions.Count;
            }

            var q = questions[index];

            return
                "Question " + (index + 1) + ":\n" + q.Q + "\n\n" +
                "A) " + q.A1 + "\n" +"B) " + q.A2 + "\n" +
                "C) " + q.A3 + "\n" +"D) " + q.A4 + "\n\n" +
                "Type A, B, C or D";
        }

        public string Answer(string input)
        {
            if (index >= questions.Count)
                return "Quiz already completed.";

            string userAnswer = input.ToLower();
            var q = questions[index];

            string result;

            if (userAnswer == q.A)
            {
                score++;

                result = "Correct!\n" + q.Explain + "\n\n";
            }
            else
            {
                result = "Incorrect!\n" + "Correct answer: " + q.A.ToUpper() + "\n\n" +q.Explain + "\n\n";
            }

            index++;

            return result + GetQuestion();
        }
    }
}
