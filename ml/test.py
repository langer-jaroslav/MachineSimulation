
import torch
from torch import nn 
from model import NeuralNet 
import pandas as pd
from pathlib import Path



MODEL_PATH = Path("models")
MODEL_NAME = "test.pth"
MODEL_SAVE_PATH = MODEL_PATH / MODEL_NAME

loaded_model = NeuralNet()
loaded_model.load_state_dict(torch.load(f=MODEL_SAVE_PATH))


file_path = './data/data_test.csv'  # Nahraďte názvem vašeho souboru
data = pd.read_csv(file_path, sep=';')
data = data[['OriginCoilResistance', 'OriginContactGap', "DebugMinPressure", "DebugMaxPressure"]].values
X_to_predict = torch.FloatTensor(data)


isOkCount = 0
loaded_model.eval()
with torch.inference_mode(): 
     for i in range(len(X_to_predict)):
          input_data = X_to_predict[i]
          _min = input_data[2].item()
          _max = input_data[3].item()          
          pred = loaded_model(input_data[:2]).numpy()[0] 
          if _min <= pred and pred <= _max:
               isOkCount = isOkCount + 1
                    
print("Celkem záznamů: " + str(len(X_to_predict)))
print("Celkem správné predikovaných: " + str(isOkCount))
print("Uspěšnost: " + str(isOkCount / len(X_to_predict) * 100))

