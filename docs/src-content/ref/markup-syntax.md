+++
date = "2016-06-22T13:41:10+02:00"
description = "The Markdown-based syntax for configuration files"
title = "Configuration Markup Syntax"
weight = 5
+++

The most of the configuration files in Bench are actually [Markdown] files.
The key-value-pairs of the configuration properties, for the whole Bench environment
or for individual apps, are written as items in unordered lists.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## Basic Syntax

The key-value-pairs are written with a colon, separating the name and the value.
Two simple properties e.g. can be written in the following form.

```Markdown
Some text block...

* Property1: Value 1
* Property2: Value 2

More text...
```

Property names must consist of alpha-numerical characters and must *not* contain whitespaces.
A value can be surrounded by angle brackets `<` and `>` if it is an URL,
or it can be surrounded by backticks if it helps the readability.

```Markdown
* Property: Value 1
* UrlProperty: <http://github.com>
* QuotedProperty: `some value`
```

## Lists and Dictionaries

If a value is a list, its items must be surrounded by backticks and separated by commas
or formatted as nested list.
If a list value is written as nested list, the values can be quoted with backtics,
angle brackets, or not at all.

```Markdown
* ListProperty1: `Value 1`, `http://value-2.com`, `Value 3`
* ListProperty2:
    + Value 1
    + <http://value-2.com>
    + `Value 3`
```

If a value is a dictionary, its items follow the rules of a list value,
but each item is a key-value-pair separated by a colon.
If the dictionary is written as a nested list, the key and the value can be quoted individually.

```Markdown
* DictionaryProperty1: `Key 1: Value`, `KEY_2: http://value-2.com`
* DictionaryProperty2:
    + Key 1: Value
    + `KEY_2`: <http://value-2.com>
```

## Property Groups

In the app libraries `res\apps.md` and `config\apps.md` the properties are grouped.
Each group of properties relates to one app.

A property group starts with an `ID` property, and ends when the list is interrupted
or ended by an empty line.
Therefore, in an app library, the properties usually look like this.

```Markdown
### App X

* ID: `AppX`
* Property1: Value 1.1
* Property2: Value 1.2

### App Y

* ID: `AppY`
* Property1: Value 2.1
* Property2: Value 2.2
```

The third-level-headlines are ignored.

## Variable Expansion

You can use placeholders for variables in property values.
A placeholder is written in dollar signs like this `$variable$`.
If a variable is grouped in a namespace, the namespace is separated
by a colon like this `$namespace:variable$`.

In the configuration files of Bench, the variables are the
configuration properties themselfs.
As a result, repetitions can be avoided.

E.g. if one property defines the path to a folder and another property
defines a sub-folder of that first folder it can be written as follows.

```Markdown
Folder1: `path\to\folder`
Folder2: `$Folder1$\sub-folder`
```

The value of `Folder2` expandeds to `path\to\folder\sub-folder`.

If a placeholder can not be resolved, its dollar signs are replaced by hash signs
to signal the error `$NotExisting$` &rarr; `#NotExisting#`.

In the app libraries `res\apps.md` and `config\apps.md`, the apps are
namespaces for their property variables.
Therefore, a placeholder for an app property is written like this:
`$AppId:PropertyName$`.

If an app property contains a placeholder for another property
of the same app, the namespace, aka the app ID, can be ommited.

```Markdown
* ID: `AppX`
* Property1: `cd`
* Property2: `ab$:Property1$ef`
```

Note the colon before the property name.
The value of `Property2` of `AppX` expandeds to `abcdef`.

Variable expansion works recursively to an arbitrary level,
but cyclic references can not be resolved.

[Markdown]: https://daringfireball.net/projects/markdown/
