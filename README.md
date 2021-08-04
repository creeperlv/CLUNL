# Creeper-Lv-s-Universal-dotNet-Library

Libraries for my programs such as servers, tools, games, etc.

# What's its goal?

To isolate some frequently used codes in my projects, and they can be updated independently. It can be useful on reducing project size.

# Distribution

You may use it freely as long as you follow the [LICENSE](./LICENSE).

## Two ways of use.

### 1. A common path to store libs.

I tend to use '\~/.libs', '\~/.libs/creeperlv', '\~/.libraries/creeperlv', '\~/.libraries/' for these paths are all available in Windows and Linux.

### 2. Include in application distribution.

To avoid user to think I am snooping his or her personal files.

# Usage

You can directly use it via NuGet, all libraries under this repo are started with `CLUNL` in NuGet Gallery.

# Design

## CLUNL

The main library. It contains data processing tools, command-line resolve utilities, wrapped IO, etc.

## CLUNL.Pipeline

A prototype pipeline implementation.

## CLUNL.Localization

It can handle language easily as long as you put language files correctly.

Also, it can be used to produce a language definition.

## CLUNL.ConsoleAppHelper

A helper for console applications, make it more standardized. It can handle parameters with multiple variants. Once you use it, ConsoleAppHelper will took in hand of processing arguments.
