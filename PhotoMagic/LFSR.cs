using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMagic
{
    class LFSR
    {
        public LFSR(string seed, int tapIndex)
        {
            int i = 0;
// tapIndex의 경우 다른 함수에서도 사용하여야하기 때문에 클래스 멤버로 tapIndex를 선언하여 저장한다.
            this.tapIndex = tapIndex;

// seed의 비트 수를 저장할 len 변수 선언.
            len = seed.Length;

// seed string 배열을 Reverse메소드를 사용하여 좌우반전시킨 다음 LSB부터 한 비트씩 foreach로 int 변수 seed에 저장한다.
            foreach (int bit in seed.Reverse())
// bit는 string 문자기 때문에 ascii code에서 48인 '0' 만큼 빼줁 다음에 원래 비트 위치만큼 left shift하여 seed에 더해준다.
                this.seed |= (bit - '0') << i++;
        }

        private int Step()
        {
// MSB는 seed에서 MSB의 위치(len - 1)만큼 1을 left shift 한 것을 and 연산하여 뽑아낸 다음 다시 첫번째 비트로 되돌린다.
            MSB = (seed & (0x01 << len - 1)) >> len - 1;

// tapIndex만큼 1을 left shift하여 seed와 & 연산을 하여 tapBit를 뽑은 다음 다시 첫번재 비트로 되돌린다.
            tapBit = (seed & (0x01 << tapIndex)) >> tapIndex;

// seed를 1만큼 left shift한다.
            seed <<= 1;

// MSB와 tapBit를 xor 하여 LSB에 들어갈 비트를 만든다.
            LSB = tapBit ^ MSB;

// seed의 1번째 비트만 LSB 비트로 교체한다.
            seed |= LSB;

            return LSB;
        }

        public int generate(int k)
        {
            int kBit = 0;

// 원래의 kBit를 두배하여(1만큼 left shif) Step()에서 return 된 LSB를 더한다(1비트를 넘어가지 않고
// kBit는 left shitf하였기 때문에 1번째 비트는 무조건 0이기 때문에 더하기를 안쓰고 or 연산으로 처리).
            for (int i = 0; i < k; i++)
                kBit = (kBit << 0x01) | Step();
            return kBit;
        }

// MSB: Most Significant Bit, LSB: Least Significant Bit
        private int MSB, LSB, seed = 0x00, tapBit = 0x01;
        private readonly int len, tapIndex;
    }
}