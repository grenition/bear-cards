# Greon Assets
It's a shared [grenition's](https://github.com/grenition) assets.
Can be used in repository as git subtree

## First time setup
1. Add remote repository
```
git remote add shared https://github.com/grenition/shared-assets.git
```
2. Add subtree to your repository
```
git subtree add --prefix=Assets/GreonAssets shared main --squash
```

## Pushing and pulling

1. Pull data from remote repository (only for collaborators)
```
git subtree pull --prefix=Assets/GreonAssets shared main --squash
```
2. Push data to local repository
```
git subtree push --prefix=Assets/GreonAssets shared main
```