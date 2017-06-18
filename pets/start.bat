#!/bin/bash
start cmd /k mongod --dbpath=data/ --port 27017
sleep 3
start cmd /k node server.js