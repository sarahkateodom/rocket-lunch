using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RocketLunch.domain.utilities
{
    public static class StringExt
    {

        public static bool IsNullOrWhiteSpace(this string @string) =>
            String.IsNullOrWhiteSpace(@string);

        public static string JoinNonEmpty(string delimiter, params string[] values) =>
            values
                .Where(value => value.IsNullOrWhiteSpace() is false)
                .ToArray()
                is var vals
                && vals.Any()
                    ? vals.Aggregate((x, y) => $"{x}{delimiter}{y}")
                    : null;

        public static Stream ToStream(this string @string) =>
            new MemoryStream(Encoding.UTF8.GetBytes(@string));

        public static byte[] ToBytes(this string @string) =>
            Encoding.UTF8.GetBytes(@string);

        public static Guid GuidParseOrEmpty(this string input) =>
            Guid.TryParse(input, out Guid result)
                ? result
                : Guid.Empty;

        public static string ToSnakeCase(this string input) =>
            input.IsNullOrWhiteSpace()
                ? input
                : Regex.Match(input, @"^_+") + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        //       ,'._,`.
        //      (-.___.-)
        //      (-.___.-)
        //      `-.___.-'                  
        //       ((  @ @|              .            __
        //        \   ` |         ,\   |`.    @|   |  |      _.-._
        //       __`.`=-=mm===mm:: |   | |`.   |   |  |    ,'=` '=`.
        //      (    `-'|:/  /:/  `/  @| | |   |, @| @|   /---)W(---\
        //       \ \   / /  / /         @| |   '         (----| |----) ,~
        //       |\ \ / /| / /            @|              \---| |---/  |
        //       | \ V /||/ /                              `.-| |-,'   |
        //       |  `-' |V /                                 \| |/    @'
        //       |    , |-'                                 __| |__
        //       |    .;: _,-.                         ,--""..| |..""--.
        //       ;;:::' "    )                        (`--::__|_|__::--')
        //     ,-"      _,  /                          \`--...___...--'/   
        //    (    -:--'/  /                           /`--...___...--'\
        //     "-._  `"'._/                           /`---...___...---'\
        //         "-._   "---.                      (`---....___....---')
        //          .' ",._ ,' )                     |`---....___....---'|
        //          /`._|  `|  |                     (`---....___....---') 
        //         (   \    |  /                      \`---...___...---'/
        //          `.  `,  ^""                        `:--...___...--;'
        //            `.,'                               `-._______.-'
    }
}