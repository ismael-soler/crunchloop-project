#!/bin/bash

set -e

# Get the database host from an environment variable
DB_HOST=${DB_HOST:-database}
DB_PORT=1433

echo "Waiting for database at $DB_HOST:$DB_PORT..."

# Using netcat to check if the port is open on the host
while ! nc -z $DB_HOST $DB_PORT; do
  # Sleep for a second and try again
  sleep 1
done

echo "Database is up, applying migrations..."
dotnet ef database update

# Execute the main command to start de API
exec "$@"