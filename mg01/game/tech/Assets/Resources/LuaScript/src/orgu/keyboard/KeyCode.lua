--KeyCode
--@author jr.zeng
--2017年4月21日 下午7:41:46
local modname = "KeyCode"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

--键盘码
KeyCode = 
    {

        None = 0,
        --The backspace key.
        Backspace = 8,
        --The tab key.
        Tab = 9,
        --The Clear key.
        Clear = 12,
        --Return key.
        Return = 13,
        --Pause on PC machines.
        Pause = 19,
        --Escape key.
        Escape = 27,
        --Space key.
        Space = 32,
        --Exclamation mark key '!'.
        Exclaim = 33,
        --Double quote key '"'.
        DoubleQuote = 34,
        --Hash key '#'.
        Hash = 35,
        --Dollar sign key '$'.
        Dollar = 36,
        --Ampersand key '&'.
        Ampersand = 38,
        --Quote key '.
        Quote = 39,
        --Left Parenthesis key '('.
        LeftParen = 40,
        --Right Parenthesis key ')'.
        RightParen = 41,
        --Asterisk key '*'.
        Asterisk = 42,
        --Plus key '+'.
        Plus = 43,
        --Comma ',' key.
        Comma = 44,
        --Minus '-' key.
        Minus = 45,
        --Period '.' key.
        Period = 46,
        --Slash '/' key.
        Slash = 47,
        --The '0' key on the top of the alphanumeric keyboard.
        Alpha0 = 48,
        --The '1' key on the top of the alphanumeric keyboard.
        Alpha1 = 49,
        --The '2' key on the top of the alphanumeric keyboard.
        Alpha2 = 50,
        --The '3' key on the top of the alphanumeric keyboard.
        Alpha3 = 51,
        --The '4' key on the top of the alphanumeric keyboard.
        Alpha4 = 52,
        --The '5' key on the top of the alphanumeric keyboard.
        Alpha5 = 53,
        --The '6' key on the top of the alphanumeric keyboard.
        Alpha6 = 54,
        --The '7' key on the top of the alphanumeric keyboard.
        Alpha7 = 55,
        --The '8' key on the top of the alphanumeric keyboard.
        Alpha8 = 56,
        --The '9' key on the top of the alphanumeric keyboard.
        Alpha9 = 57,
        --Colon ':' key.
        Colon = 58,
        --Semicolon ';' key.
        Semicolon = 59,
        --Less than '<' key.
        Less = 60,
        --Equals '=' key.
        Equals = 61,
        --Greater than '>' key.
        Greater = 62,
        --Question mark '?' key.
        Question = 63,
        --At key '@'.
        At = 64,
        --Left square bracket key '['.
        LeftBracket = 91,
        --Backslash key '\'.
        Backslash = 92,
        --Right square bracket key ']'.
        RightBracket = 93,
        --Caret key '^'.
        Caret = 94,
        --Underscore '_' key.
        Underscore = 95,
        --Back quote key '`'.
        BackQuote = 96,
        --'a' key.
        A = 97,
        -- 'b' key.
        B = 98,
        --'c' key.
        C = 99,
        -- 'd' key.
        D = 100,
        -- 'e' key.
        E = 101,
        --  'f' key.
        F = 102,
        --  'g' key.
        G = 103,
        -- 'h' key.
        H = 104,
        --  'i' key.
        I = 105,
        --  'j' key.
        J = 106,
        --  'k' key.
        K = 107,
        --  'l' key.
        L = 108,
        --  'm' key.
        M = 109,
        -- 'n' key.
        N = 110,
        -- 'o' key.
        O = 111,
        -- 'p' key.
        P = 112,
        --  'q' key.
        Q = 113,
        -- 'r' key.
        R = 114,
        --  's' key.
        S = 115,
        -- 't' key.
        T = 116,
        --  'u' key.
        U = 117,
        -- 'v' key.
        V = 118,
        --  'w' key.
        W = 119,
        --  'x' key.
        X = 120,
        --  'y' key.
        Y = 121,
        --  'z' key.
        Z = 122,
        --   The forward delete key.
        Delete = 127,
        --  Numeric keypad 0.
        Keypad0 = 256,
        -- Numeric keypad 1.
        Keypad1 = 257,
        --  Numeric keypad 2.
        Keypad2 = 258,
        --  Numeric keypad 3.
        Keypad3 = 259,
        --  Numeric keypad 4.
        Keypad4 = 260,
        --  Numeric keypad 5.
        Keypad5 = 261,
        --  Numeric keypad 6.
        Keypad6 = 262,
        --  Numeric keypad 7.
        Keypad7 = 263,
        --  Numeric keypad 8.
        Keypad8 = 264,
        -- Numeric keypad 9.
        Keypad9 = 265,
        -- Numeric keypad '.'.
        KeypadPeriod = 266,
        --  Numeric keypad '/'.
        KeypadDivide = 267,
        --  Numeric keypad '*'.
        KeypadMultiply = 268,
        --Numeric keypad '-'.
        KeypadMinus = 269,
        --Numeric keypad '+'.
        KeypadPlus = 270,
        --Numeric keypad enter.
        KeypadEnter = 271,
        --Numeric keypad '='.
        KeypadEquals = 272,
        --Up arrow key.
        UpArrow = 273,
        --Down arrow key.
        DownArrow = 274,
        --Right arrow key.
        RightArrow = 275,
        --Left arrow key.
        LeftArrow = 276,
        --Insert key key.
        Insert = 277,
        --Home key.
        Home = 278,
        --End key.
        End = 279,
        --Page up.
        PageUp = 280,
        --Page down.
        PageDown = 281,
        --F1 function key.
        F1 = 282,
        --F2 function key.
        F2 = 283,
        --F3 function key.
        F3 = 284,
        --F4 function key.
        F4 = 285,
        --F5 function key.
        F5 = 286,
        --F6 function key.
        F6 = 287,
        --F7 function key.
        F7 = 288,
        --F8 function key.
        F8 = 289,
        --F9 function key.
        F9 = 290,
        --F10 function key.
        F10 = 291,
        --F11 function key.
        F11 = 292,
        --F12 function key.
        F12 = 293,
        --F13 function key.
        F13 = 294,
        --F14 function key.
        F14 = 295,
        -- F15 function key.
        F15 = 296,
        --Numlock key.
        Numlock = 300,
        --Capslock key.
        CapsLock = 301,
        -- Scroll lock key.
        ScrollLock = 302,
        --Right shift key.
        RightShift = 303,
        --Left shift key.
        LeftShift = 304,
        --Right Control key.
        RightControl = 305,
        --Left Control key.
        LeftControl = 306,
        --Right Alt key.
        RightAlt = 307,
        --Left Alt key.
        LeftAlt = 308,
        --Right Command key.
        RightCommand = 309,
        --Right Command key.
        RightApple = 309,
        --Left Command key.
        LeftCommand = 310,
        --Left Command key.
        LeftApple = 310,
        --Left Windows key.
        LeftWindows = 311,
        --Right Windows key.
        RightWindows = 312,
        --Alt Gr key.
        AltGr = 313,
        --Help key.
        Help = 315,
        --Print key.
        Print = 316,
        --Sys Req key.
        SysReq = 317,
        --Break key.
        Break = 318,
        --Menu key.
        Menu = 319,
        Mouse0 = 323,
        Mouse1 = 324,
        Mouse2 = 325,
        Mouse3 = 326,
        Mouse4 = 327,
        Mouse5 = 328,
        Mouse6 = 329,
    }
    
    
KeyCode2Name = {}
for k,v in pairs(KeyCode) do
    KeyCode2Name[v] = k
end
