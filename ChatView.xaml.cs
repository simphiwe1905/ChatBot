using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace ChatBot.Views
{
    public partial class ChatView : UserControl
    {
        private string userName = string.Empty;
        private bool isNameCaptured = false;
        private bool hasShownTopicMenu = false;
        private bool inEnhancedChat = false;
        private string pendingTopic = string.Empty;

        private static readonly Dictionary<string, string> sentimentResponses = new()
        {
           { "happy",       "Wonderful to see you in high spirits! Let’s ride that positivity as we talk security." },
            { "good",        "Glad you’re doing well! How can I support your cybersecurity needs today?" },
            { "great",       "Fantastic! Since you're feeling great, which security topic would you like to explore?" },
            { "well",        "I’m pleased you’re doing well. What area of security shall we dig into together?" },
            { "fine",        "Nice to hear you’re fine. How can I assist with your security concerns today?" },
            { "alright",     "Alright then! Fire away with your cybersecurity questions." },
            { "okay",        "Okay! Let me know which security subjects you’d like to discuss." },
            { "excellent",   "Excellent mood! Perfect time to dive into some security knowledge." },
            { "awesome",     "Awesome energy! Let’s channel that into strengthening your security awareness." },
            { "perfect",     "Perfect! Let’s aim to make your digital defenses just as flawless." },

            { "bad",         "Sorry your day’s not going well. Maybe some security tips can help brighten it up." },
            { "sad",         "I’m sorry you’re feeling down. Cybersecurity can seem tough, but I’m here to guide you." },
            { "worried",     "It’s normal to worry. Let me share advice to help you stay protected online." },
            { "angry",       "I understand the frustration. Let’s tackle this security issue together." },
            { "confused",    "No worries—cybersecurity can be tricky. I’ll break it down clearly for you." },
            { "scared",      "Your concerns are valid. Let’s take practical steps to boost your protection." },

            { "excited",     "Love the excitement! Let’s put that enthusiasm to work learning security." },
            { "curious",     "Curiosity is key to staying safe. What would you like to learn first?" },
            { "interested",  "Great to see your interest! Let’s dive right into some security topics." },
            { "eager",       "Your eagerness is awesome! Let’s satisfy that curiosity with cybersecurity insights." },

            { "bored",       "Bored? How about sharpening your security skills with a fun tip or quick quiz?" },
            { "tired",       "Long day? Cyber‑threats never sleep! Want a light browsing‑safety tip?" },
            { "anxious",     "Feeling anxious online is common. A quick security checklist might help—interested?" },
            { "stressed",    "Stress happens, but you can regain control of your digital world. Let me help." },

            { "tell me a joke", "Why did the hacker split from the Wi‑Fi? …Because the connection just wasn’t secure anymore!" },
            { "who are you",    "I’m CyberBot—your friendly security sidekick, here to keep you safe online." },
            { "what can you do","I can assist with passwords, phishing, VPNs, malware, hacked accounts, and more. Ask away!" },
            { "daily tip",      "Cyber Tip of the Day:\nThink before you click—hover over links to inspect the URL before logging in." },
            { "give me motivation", "“Security is a journey, not a destination.” – Bruce Schneier\nStay vigilant, stay secure!" }

        };

        public ChatView()
        {
            InitializeComponent();
            Loaded += ChatView_Loaded;
        }

        private void ChatView_Loaded(object sender, RoutedEventArgs e)
        {
            AddMessageToChat("CyberBot", "Welcome to the Cybersecurity Assistant.", Brushes.Black);
            AddMessageToChat("CyberBot", "Enter your full name?", Brushes.Black);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            AddMessageToChat("You", userInput, Brushes.Black);
            string botResponse = GetBotResponse(userInput);
            AddMessageToChat("CyberBot", botResponse, Brushes.DarkRed);

            UserInputBox.Text = string.Empty;
        }

        private void AddMessageToChat(string senderName, string message, Brush background)
        {
            var border = new Border
            {
                Background = background,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 600,
                Child = new TextBlock
                {
                    Text = $"{senderName}: {message}",
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };

            ChatList.Children.Add(border);
        }

        private string GetBotResponse(string input)
        {
            input = input.ToLower();

            if (!isNameCaptured)
            {
                userName = input;
                isNameCaptured = true;
                return $"Nice to meet you, {userName}! Here's what I can help you with today:\n\n" +
                       "1. Password security and generation\n" +
                       "2. Phishing detection and prevention\n" +
                       "3. Safe browsing practices\n" +
                       "4. General cybersecurity tips\n" +
                       "5. Emergency security actions\n" +
                       "6. Exit\n" +
                       "7. Engage in a deeper cybersecurity chat";
            }

            if (inEnhancedChat)
            {
                if (input.Contains("exit"))
                {
                    inEnhancedChat = false;
                    return " Exiting enhanced chat. Let’s go back to the main menu.";
                }

                if (!string.IsNullOrEmpty(pendingTopic) && (input.Contains("yes") || input.Contains("sure") || input.Contains("please")))
                {
                    string expanded = TriggerTopicExpansion(pendingTopic);
                    pendingTopic = string.Empty;
                    return expanded + "\n\nI hope I have helped you with your request. You can now choose another topic to explore.";
                }

                return HandleEnhancedConversation(input);
            }

            return input switch
            {
                "1" => " Password Tips:\n- Use at least 12 characters mixing letters, numbers, and symbols\n- Avoid dictionary words and personal info\n- Use a password manager\n- Change passwords regularly\n- Enable 2FA for critical accounts\n\nI hope I have helped you with your password security request. You can now choose another topic to explore.",
                "2" => " Phishing Awareness:\n- Check sender address before clicking\n- Hover over links before opening them\n- Avoid urgent requests or too-good-to-be-true offers\n- Never provide personal info via email\n- Use anti-phishing filters\n\nI hope I have helped you with your phishing awareness request. You can now choose another topic to explore.",
                "3" => " Safe Browsing Tips:\n- Use secure HTTPS connections\n- Update browser and extensions regularly\n- Avoid suspicious sites and downloads\n- Disable autofill for sensitive fields\n- Use a VPN on public Wi-Fi\n\nI hope I have helped you with your safe browsing request. You can now choose another topic to explore.",
                "4" => " Cybersecurity Essentials:\n- Unique password for each site\n- 2FA everywhere\n- Regular updates\n- Backups using 3-2-1 rule\n- Beware of social engineering\n\nI hope I have helped you with your cybersecurity essentials request. You can now choose another topic to explore.",
                "5" => " Emergency Protocol:\n1. Disconnect from the internet\n2. Change passwords from a secure device\n3. Contact your IT or security team\n4. Monitor accounts for suspicious activity\n5. Consider freezing your credit report\n\nI hope I have helped you with your emergency response request. You can now choose another topic to explore.",
                "6" or "exit" => GetFarewellMessage(),
                "7" => StartEnhancedChat(),
                _ => " I didn't understand that. Please choose a number from 1 to 7 or type 'exit'."
            };
        }

        private string GetFarewellMessage()
        {
            string[] farewells = {
                "Remember to always think before you click!",
                "Stay vigilant against online threats!",
                "Your security is worth the extra effort!",
                "Keep your digital life safe and secure!"
            };
            var rnd = new Random();
            return $" Goodbye, {userName}! {farewells[rnd.Next(farewells.Length)]}";
        }

        private string StartEnhancedChat()
        {
            inEnhancedChat = true;
            return $" Let's dive deeper, {userName}! Tell me what’s on your mind, and I’ll try to help. You can say things like ‘I'm worried about phishing’ or ‘How do I stay safe online?’\n(Type 'exit' anytime to return to the menu.)";
        }

        private string HandleEnhancedConversation(string input)
        {
            foreach (var sentiment in sentimentResponses)
            {
                if (input.Contains(sentiment.Key))
                    return sentiment.Value;
            }

            if (input.Contains("phishing"))
            {
                pendingTopic = "phishing";
                return " Phishing attacks can trick even tech-savvy users. Watch for misspelled URLs, strange attachments, or unexpected messages. Always verify before clicking. Would you like tips to identify them better?";
            }
            else if (input.Contains("password"))
            {
                pendingTopic = "password";
                return " Passwords are your first defense. Use passphrases like 'PurpleGiraffe$Dance2025'. Never reuse passwords across accounts. Want a list of best practices?";
            }
            else if (input.Contains("vpn"))
            {
                pendingTopic = "vpn";
                return " VPNs protect your internet traffic, especially on public Wi-Fi. Choose one with a no-logs policy and fast servers. Need help picking one?";
            }
            else if (input.Contains("malware"))
            {
                pendingTopic = "malware";
                return " Malware can enter through email, downloads, or ads. Run antivirus tools weekly and avoid shady links. Want a checklist to stay protected?";
            }
            else if (input.Contains("hacked"))
            {
                pendingTopic = "hacked";
                return " If you think you've been hacked, change your passwords immediately, enable 2FA, and scan your device. Do you need recovery steps?";
            }
            else if (input.Contains("social media"))
            {
                pendingTopic = "social media";
                return " Social media can leak personal data. Use privacy settings, avoid location tags, and don’t overshare. Want a guide to secure your accounts?";
            }
            else if (input.Contains("wifi") || input.Contains("network"))
            {
                pendingTopic = "wifi";
                return " Home networks must be secured. Change default router passwords, use WPA3 encryption, and set a guest network. Need full setup steps?";
            }
            else if (input.Contains("how are you"))
            {
                return " I'm functioning securely and ready to assist! What's on your mind today?";
            }

            return "That's interesting. Can you tell me more so I can assist better? (Remember you can type 'exit' to return to the menu.)";
        }

        private string TriggerTopicExpansion(string topic)
        {
            return topic switch
            {
                "phishing" => " Phishing Tips:\n- Don't trust urgent emails asking for sensitive info\n- Hover over links before clicking\n- Verify the sender address\n- Look for poor grammar or logos\n- Use email filters and antivirus tools",
                "password" => " Password Best Practices:\n- Use a passphrase with 12+ characters\n- Never reuse the same password\n- Use a password manager\n- Enable 2FA\n- Change passwords regularly",
                "vpn" => " VPN Safety Tips:\n- Choose trusted providers (no-logs policy)\n- Use when on public WiFi\n- Avoid free VPNs\n- Check for DNS/IP leak protection\n- Enable kill switch option",
                "malware" => " Malware Prevention:\n- Run antivirus scans weekly\n- Avoid opening unknown attachments\n- Don't install software from sketchy sites\n- Keep OS and apps updated\n- Back up files regularly",
                "hacked" => " Hacked Response Guide:\n- Change passwords immediately\n- Enable 2FA everywhere\n- Scan for malware\n- Review recent logins on accounts\n- Notify affected services",
                "social media" => " Social Media Protection:\n- Set profiles to private\n- Avoid oversharing personal info\n- Use strong, unique passwords\n- Disable location tags\n- Regularly review privacy settings",
                "wifi" => " WiFi Security Steps:\n- Rename default network name\n- Set strong WiFi password\n- Enable WPA3 encryption\n- Create guest network\n- Update router firmware",
                _ => " General Tip: Always stay informed and use common sense online."
            };
        }
        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }
    }
}
