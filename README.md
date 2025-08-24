# JumpAndRun
## Installation
### NixOS
Add following codeblock to your `configuration.nix`.
```nix
  environment.systemPackages = with pkgs; [
    # spaceshooter
    (import (builtins.fetchurl {
      url = "https://github.com/Laurids33/JumpAndRun/releases/download/release_v1.00.02/package.nix";
      sha256 = "0dn5h24bz2va9z9zwqzc7bjp0nhga8plkgs8prlx8bq85yph95jk";
    }) {
      inherit pkgs;
      inherit (pkgs) stdenv fetchurl lib unzip steam-run;
    })
  ];
```
Use either the .desktop file or run `jump-and-run`.
