#!/bin/bash
cd source

dotnet test -c Release

if [ $? -eq 0 ]; then
    dotnet publish PurchaseCartService/PurchaseCartService.csproj -c Release -o ../publish
else
    exit 1
fi