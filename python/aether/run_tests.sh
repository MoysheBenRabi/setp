./clean.sh
export PYTHONPATH=${PYTHONPATH}:.
python src/tests/collections_tests.py
python src/tests/messaging_tests.py
python src/tests/packet_tests.py
python src/tests/session_tests.py
python src/tests/transmitter_tests.py
python src/tests/utility_tests.py
