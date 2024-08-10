#arguments
ARG ZOOKEEPER_VERSION
ARG ZOOKEEPER_TICK_TIME
ARG ZOOKEEPER_CLIENT_PORT

# zookeeper/Dockerfile
FROM confluentinc/cp-zookeeper:${ZOOKEEPER_VERSION}

# Copy any custom configuration if needed
# COPY zookeeper.properties /etc/kafka/zookeeper.properties

# Set environment variables
ENV ZOOKEEPER_CLIENT_PORT=${ZOOKEEPER_CLIENT_PORT}
ENV ZOOKEEPER_TICK_TIME=${ZOOKEEPER_TICK_TIME}

EXPOSE 2181

