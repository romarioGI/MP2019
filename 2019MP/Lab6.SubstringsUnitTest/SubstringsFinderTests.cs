using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab6.Substrings;

namespace Lab6.SubstringsUnitTest
{
    static class Tester
    {
        private static void Test(ISubstringsFinder algo, string text, string pattern, List<int> expected)
        {
            var actual = algo.FindAll(text, pattern);

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        public static void EmptyPatternEmptyText(ISubstringsFinder algo)
        {
            Test(algo, "", "", new List<int>());
        }

        public static void EmptyPattern(ISubstringsFinder algo)
        {
            Test(algo, "not empty text", "", new List<int>());
        }

        public static void EmptyText(ISubstringsFinder algo)
        {
            Test(algo, "", "not empty pattern", new List<int>());
        }

        public static void ManyTimesRepeatedA(ISubstringsFinder algo)
        {
            var text = new string('a', (int) 1e6);
            var pattern = new string('a', (int) 1e4);

            var expected = new List<int>();

            for (var i = 0; i <= text.Length - pattern.Length; i++)
                expected.Add(i);

            Test(algo, text, pattern, expected);
        }

        public static void ManyTimesRepeatedPattern(ISubstringsFinder algo)
        {
            var pattern = "колоколколокол";

            var text = pattern;
            for (var i = 0; i < 20; i++)
                text += text;

            var expected = new List<int>();
            var delta = pattern.Length / 2;
            for (var i = 0; i <= text.Length - pattern.Length; i += delta)
                expected.Add(i);

            Test(algo, text, pattern, expected);
        }

        public static void PatternLongerThanText(ISubstringsFinder algo)
        {
            var pattern = "колоколколокол";

            var text = "кол";

            var expected = new List<int>();

            Test(algo, text, pattern, expected);
        }

        public static void PatternTextAreEqual(ISubstringsFinder algo)
        {
            var pattern = "колокол";

            var text = pattern;

            var expected = new List<int> {0};

            Test(algo, text, pattern, expected);
        }

        public static void FindOneLetter(ISubstringsFinder algo)
        {
            var letter = 'р';
            var pattern = letter.ToString();

            #region text

            var text = @"Не выходи из комнаты, не совершай ошибку.
     Зачем тебе Солнце, если ты куришь Шипку?
     За дверью бессмысленно все, особенно -- возглас счастья.
     Только в уборную -- и сразу же возвращайся.

     О, не выходи из комнаты, не вызывай мотора.
     Потому что пространство сделано из коридора
     и кончается счетчиком. А если войдет живая
     милка, пасть разевая, выгони не раздевая.

     Не выходи из комнаты; считай, что тебя продуло.
     Что интересней на свете стены и стула?
     Зачем выходить оттуда, куда вернешься вечером
     таким же, каким ты был, тем более -- изувеченным?

     О, не выходи из комнаты. Танцуй, поймав, боссанову
     в пальто на голое тело, в туфлях на босу ногу.
     В прихожей пахнет капустой и мазью лыжной.
     Ты написал много букв; еще одна будет лишней.

     Не выходи из комнаты. О, пускай только комната
     догадывается, как ты выглядишь. И вообще инкогнито
     эрго сум, как заметила форме в сердцах субстанция.
     Не выходи из комнаты! На улице, чай, не Франция.

     Не будь дураком! Будь тем, чем другие не были.
     Не выходи из комнаты! То есть дай волю мебели,
     слейся лицом с обоями. Запрись и забаррикадируйся
     шкафом от хроноса, космоса, эроса, расы, вируса.";

            #endregion

            var expected = new List<int>();
            for (var i = 0; i < text.Length; i++)
                if (text[i] == letter)
                    expected.Add(i);

            Test(algo, text, pattern, expected);
        }
    }

    [TestClass]
    public class KmpTests
    {      
        [TestMethod]
        public void EmptyPatternEmptyText()
        {
            Tester.EmptyPatternEmptyText(new Kmp());
        }

        [TestMethod]
        public void EmptyPattern()
        {
            Tester.EmptyPattern(new Kmp());
        }

        [TestMethod]
        public void EmptyText()
        {
            Tester.EmptyText(new Kmp());
        }

        [TestMethod]
        public void ManyTimesRepeatedA()
        {
            Tester.ManyTimesRepeatedA(new Kmp());
        }

        [TestMethod]
        public void ManyTimesRepeatedPattern()
        {
            Tester.ManyTimesRepeatedPattern(new Kmp());
        }

        [TestMethod]
        public void PatternLongerThanText()
        {
            Tester.PatternLongerThanText(new Kmp());
        }

        [TestMethod]
        public void PatternTextAreEqual()
        {
            Tester.PatternTextAreEqual(new Kmp());
        }

        [TestMethod]
        public void FindOneLetter()
        {
            Tester.FindOneLetter(new Kmp());
        }
    }

    [TestClass]
    public class BoyerMooreTests
    {
        [TestMethod]
        public void EmptyPatternEmptyText()
        {
            Tester.EmptyPatternEmptyText(new BoyerMoore());
        }

        [TestMethod]
        public void EmptyPattern()
        {
            Tester.EmptyPattern(new BoyerMoore());
        }

        [TestMethod]
        public void EmptyText()
        {
            Tester.EmptyText(new BoyerMoore());
        }

        [TestMethod]
        public void ManyTimesRepeatedA()
        {
            Tester.ManyTimesRepeatedA(new BoyerMoore());
        }

        [TestMethod]
        public void ManyTimesRepeatedPattern()
        {
            Tester.ManyTimesRepeatedPattern(new BoyerMoore());
        }

        [TestMethod]
        public void PatternLongerThanText()
        {
            Tester.PatternLongerThanText(new BoyerMoore());
        }

        [TestMethod]
        public void PatternTextAreEqual()
        {
            Tester.PatternTextAreEqual(new BoyerMoore());
        }

        [TestMethod]
        public void FindOneLetter()
        {
            Tester.FindOneLetter(new BoyerMoore());
        }
    }
}
