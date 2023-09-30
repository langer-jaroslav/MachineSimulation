import torch
import pandas as pd
import torch.nn as nn
import torch.optim as optim
import numpy as np
from sklearn.model_selection import train_test_split
import matplotlib.pyplot as plt
from pathlib import Path
from model import NeuralNet

# Načtení dat ze souboru DSV (např. s oddělovačem ";")
file_path = './data/data_train.csv'  # Nahraďte názvem vašeho souboru
data = pd.read_csv(file_path, sep=';')

data = data.drop(["Result", "DebugMinPressure", "DebugMaxPressure"], axis=1)

# Příprava dat
X = data[['OriginCoilResistance', 'OriginContactGap']].values
y = data['PistonPressure'].values

# Rozdělení dat na trénovací a testovací množiny
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Převod dat na tenzory
X_train = torch.FloatTensor(X_train)
y_train = torch.FloatTensor(y_train)
X_test = torch.FloatTensor(X_test)
y_test = torch.FloatTensor(y_test)


# Inicializace modelu a optimizeru
model = NeuralNet()
optimizer = optim.Adam(model.parameters(), lr=0.0001)
criterion = nn.MSELoss()

# Trénování modelu
num_epochs = 800
train_losses = []
test_losses = []

for epoch in range(num_epochs):
    model.train()
    optimizer.zero_grad()
    outputs = model(X_train)
    loss = criterion(outputs, y_train.view(-1, 1))
    loss.backward()
    optimizer.step()
    train_losses.append(loss.item())

    model.eval()
    with torch.no_grad():
        test_outputs = model(X_test)
        test_loss = criterion(test_outputs, y_test.view(-1, 1))
        test_losses.append(test_loss.item())

    print(f'Epoch [{epoch + 1}/{num_epochs}], Train Loss: {loss.item():.4f}, Test Loss: {test_loss.item():.4f}')

# Vizualizace průběhu trénování
plt.figure(figsize=(10, 6))
plt.plot(train_losses, label='Train Loss')
plt.plot(test_losses, label='Test Loss')
plt.legend()
plt.xlabel('Epoch')
plt.ylabel('Loss')
plt.title('Průběh trénování')
# plt.show()

# 1. Create models directory 
MODEL_PATH = Path("models")
MODEL_PATH.mkdir(parents=True, exist_ok=True)

# 2. Create model save path 
MODEL_NAME = "test.pth"
MODEL_SAVE_PATH = MODEL_PATH / MODEL_NAME

# 3. Save the model state dict 
print(f"Ukládám vytrénovaný model: {MODEL_SAVE_PATH}")
torch.save(obj=model.state_dict(), # only saving the state_dict() only saves the models learned parameters
           f=MODEL_SAVE_PATH)
