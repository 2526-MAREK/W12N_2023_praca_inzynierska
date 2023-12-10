import paho.mqtt.client as paho
import time
import sys
import threading

# Callbacks
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT Broker!")
    else:
        print(f"Failed to connect, return code {rc}\n")

def on_publish(client, userdata, mid):
    print(f"Message {mid} published!")


def publish_status(topic, max_number, min_number, sleep_time, device):
    incrementing = True
    try:
        while True:
            payload = device
            ret = client.publish(topic, payload, qos=1)
            ret.wait_for_publish()
            print(f"Published {payload} to '{topic}' topic")

            # Aktualizacja wartości 'device'
            if incrementing:
                device += 1
                if device >= max_number:
                    incrementing = False
            else:
                device -= 1
                if device <= min_number:
                    incrementing = True  

            time.sleep(sleep_time)
    except Exception as e:
        print(f"An error occurred: {e}")

global running
def main_loop():
    while running:
        # Tu umieść kod, który ma się wykonywać w pętli
        print("Program działa...")
        time.sleep(1)  # Przykładowe opóźnienie

def wait_for_enter():
    input("Naciśnij Enter, aby zakończyć program.\n")
    running = False

# Create a client instance and assign the callbacks.
client = paho.Client("publisher_client")
client.on_connect = on_connect
client.on_publish = on_publish

# Connect to the broker.
try:
    client.connect("localhost", 1883, 60)
except Exception as e:
    print(f"Could not connect to MQTT broker! Error: {e}")
    sys.exit(-1)

client.loop_start()

# Lista tematów
topics = [
    "airSystemPoint_1/rpmSensor_1",
    "airSystemPoint_1/tempSensor_1",      #historabele
    "airSystemPoint_1/pressSensor_3",
    "airSystemPoint_1/pressSensor_1",      #historabele
    "airSystemPoint_1/tempSensor_4",
    "collingSystemPoint_1/pressSensor_5",    #historabele
    "collingSystemPoint_1/tempSensor_5",      #historabele
    "airSystemPoint_1/rpmSensor_2",
    "airSystemPoint_1/pressSensor_4",
    "airSystemPoint_1/controller_1",
    "airSystemPoint_1/pressSensor_2",
    "airSystemPoint_1/tempSensor_2",
    "airSystemPoint_1/tempSensor_3"
]

# Maksymalne i minimalne wartości dla każdego tematu
max_numbers = [60] * len(topics)
min_numbers = [52] * len(topics)

# Czas opóźnienia dla każdego wątku
sleep_times = [5] * len(topics)

# Wartości początkowe dla każdego wątku
initial_values = [52] * len(topics)

index = topics.index("airSystemPoint_1/pressSensor_4")
sleep_times[index] = 1
min_numbers[index] = 0

index = topics.index("airSystemPoint_1/pressSensor_1")

# aktualizacja max_number dla tego tematu
sleep_times[index] = 2
min_numbers[index] = 40

index = topics.index("collingSystemPoint_1/tempSensor_5")
sleep_times[index] = 1
min_numbers[index] = 40

index = topics.index("airSystemPoint_1/tempSensor_4")
sleep_times[index] = 1
min_numbers[index] = 40

try:


    for i in range(len(topics)):
        thread = threading.Thread(target=publish_status, args=(topics[i], max_numbers[i], min_numbers[i], sleep_times[i], initial_values[i]))
        thread.start()

    main_thread = threading.Thread(target=main_loop)
    main_thread.start()

    # Utworzenie i uruchomienie wątku nasłuchującego
    input_thread = threading.Thread(target=wait_for_enter)
    input_thread.start()

    # Czekanie na zakończenie wątku nasłuchującego
    input_thread.join()


except KeyboardInterrupt:
    print("Publishing was interrupted by the user.")

except Exception as e:
    print(f"An error occurred: {e}")

finally:
    client.loop_stop()
    client.disconnect()
    print("Disconnected from MQTT broker.")