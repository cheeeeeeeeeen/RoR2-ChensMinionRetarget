![Version](https://img.shields.io/badge/Version-1.0.7-orange)
![Build](https://github.com/cheeeeeeeeeen/RoR2-ChensMinionRetarget/workflows/Build/badge.svg)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Support Chen](https://img.shields.io/badge/Support-Chen-ff69b4)](https://ko-fi.com/cheeeeeeeeeen)

[![GitHub issues](https://img.shields.io/github/issues/cheeeeeeeeeen/RoR2-ChensMinionRetarget)](https://github.com/cheeeeeeeeeen/RoR2-ChensMinionRetarget/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/cheeeeeeeeeen/RoR2-ChensMinionRetarget)](https://github.com/cheeeeeeeeeen/RoR2-ChensMinionRetarget/pulls)
![Maintenance Status](https://img.shields.io/badge/Maintenance-Inactive-orange)

# Chen's Minion Retarget

## Description

Assign a priority target for your minions by pinging enemies. They will only acknowledge this command from their owner/master.

Take note that assigning a priority target for minions **does not force them to switch** targets. They will efficiently only attack the priority target **when in range of their attack** and **if it is in their line of sight**. They will also **ignore enemies that are attacking them** if the priority target is within their sights.

## Installation

Use **[r2modman](https://thunderstore.io/package/ebkr/r2modman/)** mod manager to install this mod.

If one does not want to use a mod manager, then get the DLL from **[Thunderstore](https://thunderstore.io/package/Chen/ChensMinionRetarget/)**.

## Contact
- Issue Page: https://github.com/cheeeeeeeeeen/RoR2-ChensMinionRetarget/issues
- Email: `blancfaye7@gmail.com`
- Discord: `Chen#1218`
- RoR2 Modding Server: https://discord.com/invite/5MbXZvd
- Give a tip through Ko-fi: https://ko-fi.com/cheeeeeeeeeen

## Changelog

**1.0.7**
- Thanks to KingEnderBrine, a bad conditional bug was found. Mod is also updated so that it works on the latest version of the game.

**1.0.6**
- Fix a bug where the Obedience component exists but no target was set yet. This led to a Null Reference Exception.

**1.0.5**
- Update dependency so that looping of minions from ChensHelpers will cater to this mod's features.

**1.0.4**
- Recompile once again so that it can use the breaking changes of ChensHelpers.

**1.0.3**
- Recompile so that it can use the breaking changes of ChensHelpers.

**1.0.2**
- Integrate ChensHelpers.
- There should be no notable change except for a cleaner code.

**1.0.1**
- Rollback to `2.5.14` of R2API.

**1.0.0**
- Implement the mod.